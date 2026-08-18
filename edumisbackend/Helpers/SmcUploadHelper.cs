using edumis.Models;

namespace edumisbackend.Helpers;

public sealed class FileUploadOptions {
    public long MaxFileSizeBytes { get; init; } = 10 * 1024 * 1024; // 10 MB

    public HashSet<string> AllowedExtensions { get; init; } =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".pdf" };

    public HashSet<string> AllowedMimeTypes { get; init; } =
        new(StringComparer.OrdinalIgnoreCase) {
            "image/jpeg",
            "image/png",
            "application/pdf"
        };
}

public sealed class SmcFileUploadHelper(IHostEnvironment environment, IConfiguration configuration) {
    
    private readonly FileUploadOptions options = new();

    // This application does not need interactive PDF features. Reject the common
    // PDF action and JavaScript dictionaries rather than serving potentially
    // active documents to users.
    private static readonly byte[][] ForbiddenPdfContentMarkers = [
        "<script"u8.ToArray(),
        "</script"u8.ToArray(),
        "/javascript"u8.ToArray(),
        "/js"u8.ToArray(),
        "/openaction"u8.ToArray(),
        "/aa"u8.ToArray(),
        "/launch"u8.ToArray(),
        "/submitform"u8.ToArray(),
        "/gotor"u8.ToArray(),
        "/richmedia"u8.ToArray(),
        "/embeddedfile"u8.ToArray()
    ];

    public async Task<UploadedFileDetailsModel> UploadFile(IFormFile file, string module, string forSession , string branchId ) {

        var extension = Path.GetExtension(file.FileName);
        var folderRelativePath = GetRelativePath(module, forSession, branchId);
        
        var uploadFolderPath = Path.Combine(environment.ContentRootPath, configuration["UploadPath"] ?? "uploads", folderRelativePath);
        if (!Directory.Exists(uploadFolderPath))
            Directory.CreateDirectory(uploadFolderPath);            
        
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fileFullPath = Path.Combine(uploadFolderPath, fileName);

        await using (var stream = new FileStream(fileFullPath, FileMode.Create))
            await file.CopyToAsync(stream);
            
        return new UploadedFileDetailsModel {
            FileName = fileName,
            FileExtension = extension,
            FileMimeType = file.ContentType.ToLowerInvariant(),
            FilePath = folderRelativePath
        };
    }

    public async Task<FileValidationResult> ValidateAsync( IFormFile? file, CancellationToken cancellationToken = default) {
        
        if (file is null || file.Length <= 0)
            return FileValidationResult.Fail("File is empty.");

        if (file.Length > options.MaxFileSizeBytes)
            return FileValidationResult.Fail($"File size cannot exceed {options.MaxFileSizeBytes / 1024 / 1024} MB.");

        var originalFileName = Path.GetFileName(file.FileName);

        if (string.IsNullOrWhiteSpace(originalFileName))
            return FileValidationResult.Fail("Invalid filename.");

        if (HasMultipleExtensions(originalFileName))
            return FileValidationResult.Fail("Multiple file extensions are not allowed.");

        // Validate file extension
        var extension = Path.GetExtension(originalFileName);

        if (string.IsNullOrWhiteSpace(extension))
            return FileValidationResult.Fail("File must have an extension.");

        if (!options.AllowedExtensions.Contains(extension))
            return FileValidationResult.Fail($"File type '{extension}' is not allowed.");
        
        if (string.IsNullOrWhiteSpace(file.ContentType))
            return FileValidationResult.Fail("File MIME type is missing.");

        // Verify MIME Type
        if (!options.AllowedMimeTypes.Contains(file.ContentType))
            return FileValidationResult.Fail($"MIME type '{file.ContentType}' is not allowed.");

       
        // Verify actual file content / magic bytes
       await using var stream = file.OpenReadStream();

        var actualFileType = await DetectFileTypeByContentAsync(stream, cancellationToken);

        if (actualFileType is null)
            return FileValidationResult.Fail("Unable to determine the actual file type.");

        if (!IsExtensionCompatible(extension, actualFileType.Value))
            return FileValidationResult.Fail("File content does not match its extension.");

        if (actualFileType == DetectedFileType.Pdf &&
            await ContainsForbiddenPdfContentAsync(stream, cancellationToken))
        {
            return FileValidationResult.Fail("PDF files containing scripts or active content are not allowed.");
        }
        
        return FileValidationResult.Success(originalFileName, extension, file.ContentType, file.Length, actualFileType.Value);
    }

    public bool RemoveFile(string filePath, string fileName) {
        
        var uploadPath = Path.Combine(environment.ContentRootPath, configuration["UploadPath"] ??"uploads", filePath);

        if (!Directory.Exists(uploadPath)) return false;
        
        var absoluteFilePath = Path.Combine(uploadPath, fileName);
        if (File.Exists(absoluteFilePath))
            File.Delete(absoluteFilePath);
        return true;
        
    }

    private string GetRelativePath(string module, string? forSession = null, string? branchId = null, string? folderName = null ) => 
        Path.Combine(
            forSession ?? DateTime.Now.Year.ToString(), 
            module, 
            branchId ?? "", 
            folderName ?? ""
        );
    
    private static bool HasMultipleExtensions(string fileName) {
        fileName = Path.GetFileName(fileName);

        var nameWithoutFinalExtension =
            Path.GetFileNameWithoutExtension(fileName);

        return Path.HasExtension(nameWithoutFinalExtension);
    }
    
    private static async Task<DetectedFileType?> DetectFileTypeByContentAsync(Stream stream, CancellationToken cancellationToken) {
        var buffer = new byte[16];

        var bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken);

        stream.Position = 0;

        // JPEG
        if (bytesRead >= 3 &&
            buffer[0] == 0xFF &&
            buffer[1] == 0xD8 &&
            buffer[2] == 0xFF)
        {
            return DetectedFileType.Jpeg;
        }

        // PNG
        if (bytesRead >= 8 &&
            buffer[0] == 0x89 &&
            buffer[1] == 0x50 &&
            buffer[2] == 0x4E &&
            buffer[3] == 0x47 &&
            buffer[4] == 0x0D &&
            buffer[5] == 0x0A &&
            buffer[6] == 0x1A &&
            buffer[7] == 0x0A)
        {
            return DetectedFileType.Png;
        }

        // PDF
        if (bytesRead >= 5 &&
            buffer[0] == 0x25 && // %
            buffer[1] == 0x50 && // P
            buffer[2] == 0x44 && // D
            buffer[3] == 0x46 && // F
            buffer[4] == 0x2D)    // -
        {
            return DetectedFileType.Pdf;
        }

        return null;
    }

    private static bool IsExtensionCompatible(string extension, DetectedFileType actualType) => 
        extension.ToLowerInvariant() switch {
            ".jpg" or ".jpeg" => actualType == DetectedFileType.Jpeg,
            ".png" => actualType == DetectedFileType.Png,
            ".pdf" => actualType == DetectedFileType.Pdf,
            _ => false
        };

    private static async Task<bool> ContainsForbiddenPdfContentAsync(Stream stream, CancellationToken cancellationToken) {
        stream.Position = 0;

        var buffer = new byte[8192];
        var overlap = Array.Empty<byte>();

        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(), cancellationToken)) > 0) {
            var content = new byte[overlap.Length + bytesRead];
            Buffer.BlockCopy(overlap, 0, content, 0, overlap.Length);
            Buffer.BlockCopy(buffer, 0, content, overlap.Length, bytesRead);

            if (ForbiddenPdfContentMarkers.Any(marker => ContainsAsciiIgnoreCase(content, marker))) {
                stream.Position = 0;
                return true;
            }

            var overlapLength = Math.Min(16, content.Length);
            overlap = content[^overlapLength..];
        }

        stream.Position = 0;
        return false;
    }

    private static bool ContainsAsciiIgnoreCase(byte[] content, byte[] marker) {
        for (var start = 0; start <= content.Length - marker.Length; start++) {
            var matches = true;
            for (var index = 0; index < marker.Length; index++) {
                var value = content[start + index];
                if (value is >= (byte)'A' and <= (byte)'Z')
                    value = (byte)(value + ('a' - 'A'));

                if (value != marker[index]) {
                    matches = false;
                    break;
                }
            }

            if (matches)
                return true;
        }

        return false;
    }
    
}


public enum DetectedFileType {
    Jpeg, Png, Pdf
}

public sealed record FileValidationResult( bool IsValid, string? Error, string? OriginalFileName, string? Extension, 
    string? MimeType, long Size, DetectedFileType? DetectedType) {
    
    public static FileValidationResult Fail(string error) => new(
            false,
            error,
            null,
            null,
            null,
            0,
            null);

    public static FileValidationResult Success( string originalFileName, string extension, string mimeType, long size, DetectedFileType detectedType) => new(
            true,
            null,
            originalFileName,
            extension,
            mimeType,
            size,
            detectedType);
}
