namespace edumis.Models;

public class UploadedFileDetailsModel
{
    public string FileName { get; set; } = default!;
    public string? FilePath { get; set; }
    public string? FileExtension { get; set; }
    public string? FileMimeType { get; set;}

    public string? ErrorMessage { get; set; } = null;
}
