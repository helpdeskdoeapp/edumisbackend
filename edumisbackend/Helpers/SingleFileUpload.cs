using edumis.Models;

namespace edumisbackend.Helpers
{
    public class SingleFileUpload(IConfiguration _configuration, IWebHostEnvironment _environment)
    {
        #region Upload File   
        public async Task<UploadedFileDetailsModel> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string ForSession)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");
                        
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only specific file extensions are allowed.");

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Invalid file format.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(ForSession, module).Replace("\\", "/")//_configuration["UploadPath"] ?? string.Empty,
            }; // Return the uploaded file name
        }

        public async Task<UploadedFileDetailsModel> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string ForSession, string FolderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))            
                throw new ArgumentException("Only specific file extensions are allowed.");            

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))            
                throw new ArgumentException("Invalid file format.");            

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";            
           
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, FolderName);
            if (!Directory.Exists(uploadPath))            
                Directory.CreateDirectory(uploadPath);            

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(ForSession, module, FolderName).Replace("\\", "/")//_configuration["UploadPath"] ?? string.Empty,
            }; // Return the uploaded file name
        }

        public async Task<UploadedFileDetailsModel> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string ForSession, string UploadedFileName, string FolderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))            
                throw new ArgumentException("Only specific file extensions are allowed.");           

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))            
                throw new ArgumentException("Invalid file format.");            
            
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, FolderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);           

            var filePath = Path.Combine(uploadPath, UploadedFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = UploadedFileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(ForSession, module, FolderName).Replace("\\", "/")//_configuration["UploadPath"] ?? string.Empty,
            }; // Return the uploaded file name
        }        

        public async Task<UploadedFileDetailsModel> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string ForSession , string BranchId, string UploadedFileName, string FolderName)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))            
                throw new ArgumentException("Only specific file extensions are allowed.");            

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))            
                throw new ArgumentException("Invalid file format.");            

            var fileName = !string.IsNullOrEmpty(UploadedFileName) ? UploadedFileName : $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            //var currentYear = DateTime.Now.Year.ToString();
            ForSession = string.IsNullOrEmpty(ForSession) ? DateTime.Now.Year.ToString() : ForSession;
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, BranchId, FolderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);            

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(ForSession, module, BranchId, FolderName).Replace("\\", "/")//_configuration["UploadPath"] ?? string.Empty,
            }; // Return the uploaded file name
        }

        public async Task<UploadedFileDetailsModel> UploadFileInFolder(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string FolderName, string FileName = "")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only specific file extensions are allowed.");

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Invalid file format.");

            var fileName = string.IsNullOrEmpty(FileName) ? $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}" : $"{FileName}{Path.GetExtension(file.FileName)}";

            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], module, FolderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(_configuration["UploadPath"] , module, FolderName, fileName).Replace("\\", "/")
            }; // Return the uploaded file name
        }

        public async Task<UploadedFileDetailsModel> UploadFileForSession(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string forSession, string FileName = "")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only specific file extensions are allowed.");

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Invalid file format.");

            var fileName = string.IsNullOrEmpty(FileName) ? $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}" : $"{FileName}{Path.GetExtension(file.FileName)}";

            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], module, forSession);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(_configuration["UploadPath"], module, forSession, fileName).Replace("\\", "/")
            }; // Return the uploaded file name
        }

        public async Task<UploadedFileDetailsModel> UploadFileInSubFolder(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string BranchId, string SubFolderName, string FileName = "")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only specific file extensions are allowed.");

            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Invalid file format.");

            var fileName = string.IsNullOrEmpty(FileName) ? $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}" : FileName;

            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], module, BranchId, SubFolderName);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new UploadedFileDetailsModel()
            {
                FileName = fileName,
                FileExtension = fileExtension,
                FileMimeType = file.ContentType.ToLowerInvariant(),
                FilePath = Path.Combine(_configuration["UploadPath"] ?? string.Empty, module, BranchId, SubFolderName, fileName).Replace("\\", "/")

            }; // Return the uploaded file name
        }


        #region Commented Code
        //public async Task<string> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string FolderName = "")
        //{
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("File is empty.");

        //    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(fileExtension))
        //        throw new ArgumentException("Only specific file extensions are allowed.");

        //    if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        //        throw new ArgumentException("Invalid file format.");

        //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], DateTime.Now.Year.ToString(), module, FolderName);
        //    if (!Directory.Exists(uploadPath))
        //        Directory.CreateDirectory(uploadPath);

        //    var filePath = Path.Combine(uploadPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return fileName; // Return the uploaded file name
        //}

        //public async Task<string> UploadFile(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string ForSession = "", string UploadedFileName = "")
        //{
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("File is empty.");

        //    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(fileExtension))
        //    {
        //        throw new ArgumentException("Only specific file extensions are allowed.");
        //    }

        //    if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        //    {
        //        throw new ArgumentException("Invalid file format.");
        //    }

        //    var fileName = !string.IsNullOrEmpty(UploadedFileName) ? UploadedFileName : $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    //var currentYear = DateTime.Now.Year.ToString();
        //    ForSession = string.IsNullOrEmpty(ForSession) ? DateTime.Now.Year.ToString() : ForSession;
        //    var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module);
        //    if (!Directory.Exists(uploadPath))
        //    {
        //        Directory.CreateDirectory(uploadPath);
        //    }

        //    var filePath = Path.Combine(uploadPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return fileName; // Return the uploaded file name
        //}

        //public async Task<string> UploadFileInFolder(IFormFile file, string[] allowedExtensions, string[] allowedMimeTypes, string module, string FolderName, string ForSession = "", string UploadedFileName = "")
        //{
        //    if (file == null || file.Length == 0)
        //        throw new ArgumentException("File is empty.");

        //    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        //    if (!allowedExtensions.Contains(fileExtension))
        //    {
        //        throw new ArgumentException("Only specific file extensions are allowed.");
        //    }

        //    if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        //    {
        //        throw new ArgumentException("Invalid file format.");
        //    }

        //    var fileName = !string.IsNullOrEmpty(UploadedFileName) ? UploadedFileName : $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    //var currentYear = DateTime.Now.Year.ToString();
        //    ForSession = string.IsNullOrEmpty(ForSession) ? DateTime.Now.Year.ToString() : ForSession;
        //    var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, FolderName);
        //    if (!Directory.Exists(uploadPath))
        //    {
        //        Directory.CreateDirectory(uploadPath);
        //    }

        //    var filePath = Path.Combine(uploadPath, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return fileName; // Return the uploaded file name
        //}
        #endregion
        #endregion

        #region Get File Upload Paths
        //public string GetFilePath(string fileName, string module, string FolderName = "")
        //{
        //    var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], DateTime.Now.Year.ToString(), module, FolderName);
        //    var filePath = Path.Combine(uploadPath, fileName);
        //    var fileUrl = filePath.Replace(_environment.ContentRootPath, "").Replace("\\", "/");
        //    return fileUrl;
        //}

        public string GetFilePath(string fileName, string module, string ForSession, string FolderName = "")
        {
            //var currentYear = DateTime.Now.Year.ToString();
            ForSession = string.IsNullOrEmpty(ForSession) ? DateTime.Now.Year.ToString() : ForSession;
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, FolderName);
            var filePath = Path.Combine(uploadPath, fileName);
            var fileUrl = filePath.Replace(_environment.ContentRootPath, "").Replace("\\", "/");
            return fileUrl;
        }

        public string GetFilePath(string fileName, string module, string ForSession, string BranchId, string FolderName = "")
        {
            //var currentYear = DateTime.Now.Year.ToString();
            ForSession = string.IsNullOrEmpty(ForSession) ? DateTime.Now.Year.ToString() : ForSession;
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, BranchId, FolderName);
            var filePath = Path.Combine(uploadPath, fileName);
            var fileUrl = filePath.Replace(_environment.ContentRootPath, "").Replace("\\", "/");
            return fileUrl;
        }

        public string GetUploadPath(string module, string ForSession)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            return Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module)
                .Replace(_environment.ContentRootPath, "").Replace("\\", "/");
        }

        public string GetUploadPath(string module, string ForSession, string BranchId)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            return Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, BranchId)
                .Replace(_environment.ContentRootPath, "").Replace("\\", "/");
        }

        public string GetUploadPath(string module, string ForSession, string BranchId, string FolderName)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            return Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, BranchId, FolderName)
                .Replace(_environment.ContentRootPath, "").Replace("\\", "/");
        }
        #endregion

        #region Remove File
        public bool RemoveFile(string FileUrl)
        {
            var uploadPath = Path.Combine(_environment.ContentRootPath, FileUrl).Replace("\\", "/");

            if (File.Exists(uploadPath))
            {
                File.Delete(uploadPath);
                return true;
            }
            return false;
        }

        public bool RemoveFile(string FilePath, string FileName)
        {
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], FilePath).Replace("\\", "/");

            if (Directory.Exists(uploadPath))
            {
                var filePath = Path.Combine(uploadPath, FileName).Replace("\\", "/");
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return true;
            }
            return false;
        }

        public bool RemoveFile(string fileName, string module, string ForSession)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module).Replace("\\", "/");

            if (Directory.Exists(uploadPath))
            {
                var filePath = Path.Combine(uploadPath, fileName).Replace("\\", "/");
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return true;
            }
            return false;
        }
       
        public bool RemoveFile(string fileName, string module, string ForSession, string FolderName)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, FolderName).Replace("\\", "/");

            if (Directory.Exists(uploadPath))
            {
                var filePath = Path.Combine(uploadPath, fileName).Replace("\\", "/");
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return true;
            }
            return false;
        }

        public bool RemoveFile(string fileName, string module, string ForSession, string BranchId, string FolderName)
        {
            //var currentYear = DateTime.Now.Year.ToString();
            var uploadPath = Path.Combine(_environment.ContentRootPath, _configuration["UploadPath"], ForSession, module, BranchId, FolderName).Replace("\\", "/");

            if (Directory.Exists(uploadPath))
            {
                var filePath = Path.Combine(uploadPath, fileName).Replace("\\", "/");
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return true;
            }
            return false;
        }
        #endregion
    }
}
