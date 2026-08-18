using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Employees;
using edumis.Models.Employees.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Employees;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController(IUnitOfWork unitOfWork, IConfiguration configuration, SingleFileUpload singleFileUpload) : ControllerBase
{

    #region Create employee
    [HttpPost("addemployee")]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDTO EmployeeDetails)
    {
        if (EmployeeDetails == null) return Ok(ResponseModel<string>.Failure("Invalid details", StatusCodes.Status406NotAcceptable));

        var emailExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmailId == EmployeeDetails.EmailId);
        if (emailExists != null)
            return Ok(ResponseModel<string>.Failure("Employee with same Email Id already exists.", StatusCodes.Status409Conflict));
     
        if (!string.IsNullOrEmpty(EmployeeDetails.AadharNo))
        {
            if (EmployeeDetails.AadharNo.Length != 12)
                return Ok(ResponseModel<string>.Failure("Invalid Aadhar Number. 12 Digit Aadhar Number is required.", StatusCodes.Status406NotAcceptable));
          
            var aadharExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.AadharNo == EmployeeDetails.AadharNo);
            if (aadharExists != null)
                return Ok(ResponseModel<string>.Failure("Employee with same Aadhar number already exists.", StatusCodes.Status409Conflict));            
        }


        if (!string.IsNullOrEmpty(EmployeeDetails.PanNo))
        {
            if (EmployeeDetails.PanNo.Length != 10)
                return Ok(ResponseModel<string>.Failure("Invalid PAN Number. 10 Digit PAN Number is required.", StatusCodes.Status406NotAcceptable));
          
            var panExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.PanNo == EmployeeDetails.PanNo);
            if (panExists != null)
                return Ok(ResponseModel<string>.Failure("Employee with same PAN number already exists.", StatusCodes.Status409Conflict));          
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        var returnval = await unitOfWork.EmployeesRepo.CreateEmployee(EmployeeDetails, LoggedInUserId);
        if (string.IsNullOrEmpty(returnval))
            return Ok(ResponseModel<string>.Failure("Failed to add employee details", StatusCodes.Status500InternalServerError));
       
        return Ok(ResponseModel<string?>.Success(returnval, "Employee Added Successfully.", StatusCodes.Status201Created));  
    }    
    #endregion

    #region Update Employee Details
    [HttpPost("updateemployee")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDTO EmployeeDetails)
    {
        if (EmployeeDetails == null) return Ok(ResponseModel<string>.Failure("Invalid request", StatusCodes.Status406NotAcceptable));

        if (string.IsNullOrEmpty(EmployeeDetails.EmployeeId)) return Ok(ResponseModel<string>.Failure("Invalid Employee Id!", StatusCodes.Status406NotAcceptable));

        if (EmployeeDetails.EmployeeId.ToLower() == "sysadmin")
            return Ok(ResponseModel<string>.Failure("Permission denied to update employee.", StatusCodes.Status403Forbidden));
       
        var EmpDataIfExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == EmployeeDetails.EmployeeId);
        if (EmpDataIfExists != null)
        {
            if (!string.IsNullOrEmpty(EmployeeDetails.AadharNo) && EmpDataIfExists.AadharNo != EmployeeDetails.AadharNo)
            {
                if (EmployeeDetails.AadharNo.Length != 12)
                    return Ok(ResponseModel<string>.Failure("Invalid Aadhar Number. 12 Digit Aadhar Number is required.", StatusCodes.Status403Forbidden));
              
                var aadharExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.AadharNo == EmployeeDetails.AadharNo);
                if (aadharExists != null)
                    return Ok(ResponseModel<string>.Failure("Employee with same Aadhar number already exists.", StatusCodes.Status409Conflict));              
            }

            if (!string.IsNullOrEmpty(EmployeeDetails.PanNo) && EmpDataIfExists.PanNo != EmployeeDetails.PanNo)
            {
                if (EmployeeDetails.PanNo.Length != 10)
                    return Ok(ResponseModel<string>.Failure("Invalid PAN Number. 10 Digit PAN Number is required.", StatusCodes.Status406NotAcceptable));
               
                var panExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.PanNo == EmployeeDetails.PanNo);
                if (panExists != null)
                    return Ok(ResponseModel<string>.Failure("Employee with same PAN number already exists.", StatusCodes.Status409Conflict));                
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

            var returnval = await unitOfWork.EmployeesRepo.UpdateEmployee(EmployeeDetails, LoggedInUserId);
            if (!returnval)
                return Ok(ResponseModel<string>.Failure("Failed to update employee details", StatusCodes.Status500InternalServerError));

            return Ok(ResponseModel<string>.Success(string.Empty, "Employee Details Updated Successfully."));           
        }
        return Ok(ResponseModel<string>.NoData("No Employeee Details Found."));
    }

    
    [HttpPost("deactivate/{employeeid}")]
    public async Task<IActionResult> Deactivate([FromRoute] string employeeid)
    {
        if (string.IsNullOrEmpty(employeeid)) return Ok(ResponseModel<string>.Failure("Invalid request", StatusCodes.Status406NotAcceptable));

        if (employeeid.ToLower() == "sysadmin")
            return Ok(ResponseModel<string>.Failure("Permission denied to deactivate employee.", StatusCodes.Status403Forbidden));
               
        var EmpDataIfExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid);
        if (EmpDataIfExists == null)
            return Ok(ResponseModel<string>.NoData("No Data Found."));

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var returnval = await unitOfWork.EmployeesRepo.DeActivateEmployee(employeeid,
            (userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty));

        return Ok(returnval ? ResponseModel<string>.Success(string.Empty, "Employee Deactivated Successfully.") :
           ResponseModel<string>.Failure("Failed to deactivate employee."));     
    }

    [HttpPost("activate/{employeeid}")]
    public async Task<IActionResult> Activate([FromRoute] string employeeid)
    {
        if (string.IsNullOrEmpty(employeeid)) return Ok(ResponseModel<string>.Failure("Invalid request", StatusCodes.Status406NotAcceptable));
        if (employeeid.ToLower() == "sysadmin")
            return Ok(ResponseModel<string>.Failure("Permission denied to activate employee.", StatusCodes.Status403Forbidden));
             
        var EmpDataIfExists = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid);
        if (EmpDataIfExists == null)
            return Ok(ResponseModel<string>.NoData("No Data Found."));

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var returnval = await unitOfWork.EmployeesRepo.ActivateEmployee(employeeid,
            (userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty));

        return Ok(returnval ? ResponseModel<string>.Success(string.Empty, "Employee Activated Successfully.") :
            ResponseModel<string>.Failure("Failed to activate employee."));
    }
    #endregion

    #region Add/Update Employee appointment Details 
    [HttpPost("appointmentdetails")]
    public async Task<ActionResult> UpdateAppointmentDetails([FromBody] AppointmentDTO AppointmentDetails)
    {
        if (AppointmentDetails == null) return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(AppointmentDetails.EmployeeId)) return Ok(ResponseModel<string>.Failure("Invalid request", StatusCodes.Status406NotAcceptable));

        if (AppointmentDetails.EmployeeId.ToLower() == "sysadmin") return Ok(ResponseModel<string>.Failure("Permission denied to update employee.", StatusCodes.Status403Forbidden));

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        var returnval = await unitOfWork.EmployeeAppointmentRepo.UpdateAppointmentDetails(AppointmentDetails, LoggedInUserId);
        if (!returnval)
            return Ok(ResponseModel<string>.Failure("Failed to update employee appointment details", StatusCodes.Status500InternalServerError));

        return Ok(ResponseModel<string>.Success(string.Empty, "Employee Appointment Details Updated Successfully."));  
    }
    #endregion

    #region Employee Educational Details Add/Update
    [HttpPost("educationaldetails/{recordid?}")]
    public async Task<IActionResult> UpdateEducationalDetails([FromBody] EducationDTO EducationDetails, [FromRoute] long? recordid)
    {
        if (EducationDetails == null) return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(EducationDetails.EmployeeId))
            return Ok(ResponseModel<string>.Failure("Invalid request", StatusCodes.Status406NotAcceptable));
      
        if (EducationDetails.EmployeeId.ToLower() == "sysadmin")
            return Ok(ResponseModel<string>.Failure("Permission denied to update employee.", StatusCodes.Status403Forbidden));
               
        if (recordid != null && recordid != 0)
            if (await unitOfWork.EmployeeEducationRepo.GetFirstOrDefault(x => x.RowId == recordid && x.EmployeeId == EducationDetails.EmployeeId) == null)
                return Ok(ResponseModel<string>.NoData("No Data Found."));

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        var returnval = await unitOfWork.EmployeeEducationRepo.AddEducationalDetails(EducationDetails, LoggedInUserId, recordid);
        if (!returnval)
            return Ok(ResponseModel<string>.Failure("Failed to update employee education details", StatusCodes.Status500InternalServerError));

        return Ok(ResponseModel<string>.Success(string.Empty, "Employee Educational Details Updated Successfully."));        
    }


    [HttpPost("deleteeducation/{employeeid}/{recordid}")]
    public async Task<IActionResult> RemoveEducationalDetail([FromRoute] string employeeid, [FromRoute] long recordid)
    {
        if (string.IsNullOrEmpty(employeeid)) return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (employeeid.ToLower() == "sysadmin") return Ok(ResponseModel<string>.Failure("Permission denied to update employee.", StatusCodes.Status403Forbidden));

        var returnval = await unitOfWork.EmployeeEducationRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid && x.RowId == recordid);
        if (returnval == null)
            return Ok(ResponseModel<string>.NoData("No Employee Educational Data Found!"));
      
        await unitOfWork.EmployeeEducationRepo.Remove(returnval);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(string.Empty, "Employee Educational Details Removed Successfully."));
    }
    #endregion

    #region Get Employee Details

    [HttpGet("employeedetails/{employeeid}")]
    public async Task<IActionResult> GetEmployeeDetails([FromRoute] string employeeid)
    {
        if (string.IsNullOrEmpty(employeeid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var EmpDetails = await unitOfWork.EmployeesRepo.GetEmployeeDetails(employeeid);

        if (EmpDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        return Ok(ResponseModel<EmployeeDetailsDTO?>.Success(EmpDetails, "Employee details retrieved successfully."));       
    }

    [HttpGet("appointmentdetails/{employeeid}")]
    public async Task<IActionResult> GetEmployeeAppointmentDetails([FromRoute] string employeeid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(employeeid)) return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var AppDetails = await unitOfWork.EmployeeAppointmentRepo.GetAppointmentDetails(employeeid);

        if (AppDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        return Ok(ResponseModel<AppointmentDetailsDTO>.Success(AppDetails, "Appointment details retrieved successfully."));       
    }

    [HttpGet("educationdetails/{employeeid}")]
    public async Task<IActionResult> GetEducationalDetails([FromRoute] string employeeid)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(employeeid) || employeeid == "")
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var EducationDetails = await unitOfWork.EmployeeEducationRepo.GetEducationDetails(employeeid);

        if (EducationDetails == null || EducationDetails.Count == 0)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        return Ok(ResponseModel<List<EducationDetailsDTO>?>.Success(EducationDetails, "Educational details retrieved successfully."));
     
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchEmployee([FromBody] SearchEmployeeRequestDTO searchEmployee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(searchEmployee.BranchId))
        {
            var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

            string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(BranchID))
                return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

            searchEmployee.BranchId = BranchID;
        }

        var EmpDetails = await unitOfWork.EmployeesRepo.SearchEmployees(searchEmployee);

        if (EmpDetails == null || EmpDetails.Count == 0)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        if (searchEmployee.Gender.HasValue && searchEmployee.Gender > 0)
            EmpDetails = EmpDetails.Where(x => x.Gender == searchEmployee.Gender).ToList();

        if (searchEmployee.Category.HasValue && searchEmployee.Category > 0)
            EmpDetails = EmpDetails.Where(x => x.Category == searchEmployee.Category).ToList();

        if (searchEmployee.SelectionCategory.HasValue && searchEmployee.SelectionCategory > 0)
            EmpDetails = EmpDetails.Where(x => x.SelectionCategory == searchEmployee.SelectionCategory).ToList();

        if (searchEmployee.DisabilityType.HasValue && searchEmployee.DisabilityType > 0)
            EmpDetails = EmpDetails.Where(x => x.DisabilityType == searchEmployee.DisabilityType).ToList();

        if (searchEmployee.DesignationGroup.HasValue && searchEmployee.DesignationGroup > 0)
            EmpDetails = EmpDetails.Where(x => x.DesignationGroup == searchEmployee.DesignationGroup).ToList();

        if (searchEmployee.DesignationId.HasValue && searchEmployee.DesignationId > 0)
            EmpDetails = EmpDetails.Where(x => x.DesignationId == searchEmployee.DesignationId).ToList();

        if (searchEmployee.GazettedOnly.HasValue)
            EmpDetails = EmpDetails.Where(x => x.IsGazetted == searchEmployee.GazettedOnly).ToList();

        if (searchEmployee.VehiclefacilityAvailed.HasValue)
            EmpDetails = EmpDetails.Where(x => x.VehicleFacilityAvailed == searchEmployee.VehiclefacilityAvailed).ToList();

        if (searchEmployee.Status.HasValue)
            EmpDetails = EmpDetails.Where(x => x.IsActive == searchEmployee.Status).ToList();

        if (EmpDetails == null || EmpDetails.Count == 0)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var sorted = EmpDetails.OrderBy(x => x.EmployeeId);

        var paginated = sorted
            .Skip((searchEmployee.PageNo - 1) * searchEmployee.PageSize)
            .Take(searchEmployee.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<SearchResultResponseDTO>
        {
            Items = paginated,
            PageNumber = searchEmployee.PageNo,
            PageSize = searchEmployee.PageSize,
            TotalCount = EmpDetails.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<SearchResultResponseDTO>>.Success(response, "Employee details retrieved successfully"));
    }
    #endregion
        
    #region Employee Profile Image
    [HttpPost("editphoto/{employeeid}")]
    public async Task<IActionResult> UploadPhoto(IFormFile file, [FromRoute] string employeeid)
    {
        var employeeData = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid);

        if (employeeData == null)
            return Ok(ResponseModel<string>.NoData("Employee details not found!"));

        if (file == null || file.Length == 0)
            return Ok(ResponseModel<string>.Failure("Invalid photograph."));

        long maxFileSize = Convert.ToInt32(configuration["UserProfileFileSize"]) * 1024;
        if (file.Length > maxFileSize)
            return Ok(ResponseModel<string>.Failure("Invalid image file. Maximum 50KB image file is allowed!"));
        
        string[] allowedExtensions = UtilityClass.AllowedImageExtensions;
        string[] allowedMimeTypes = UtilityClass.AllowedImageMimeTypes;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
            return Ok(ResponseModel<string>.Failure("Invalid image file. Only *.JPG or &.PNG files are allowed!"));
        
        if (!allowedMimeTypes.Any(x => x.Equals(file.ContentType.ToLowerInvariant())))
            return Ok(ResponseModel<string>.Failure("Invalid image file. Only *.JPG or &.PNG files are allowed!"));

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var photo = new ProfilePhoto(
                employeeid,
                memoryStream.ToArray(),
                fileExtension,
                file.ContentType
            );

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

            var returnData = await unitOfWork.EmployeesRepo.EditPhoto(photo, LoggedInUserId);
            if (!returnData)
                return Ok(ResponseModel<string>.Failure("Unable to update profile image.", StatusCodes.Status500InternalServerError));

            return Ok(ResponseModel<string>.Success(string.Empty, "Profile image updated."));           
        }
    }

    [HttpGet("profileimage/{employeeid}")]
    public async Task<IActionResult> GetProfilePhoto([FromRoute] string employeeid)
    {
        var employeeData = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid);

        if (employeeData == null)
            return Ok(ResponseModel<string>.NoData("Employee details not found!"));

        return employeeData.Photo != null ?
             Ok(ResponseModel<FileContentResult>.Success(File(employeeData.Photo, employeeData.ContentType ?? string.Empty), "Profile image retrieved successfully.")):
             Ok(ResponseModel<string>.NoData("Employee details not found!"));       
    }
    #endregion
        
    #region Employee Achievements
    [HttpPost("addachievement")]
    public async Task<IActionResult> AddAchievement([FromForm] EmployeeAchievementRequestDTO requestData, IFormFile DocumentFile = null)
    {
        long pdfFileSizeAllowed = Convert.ToInt64(configuration["PDFUploadFileSize"]);
        if (requestData == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        UploadedFileDetailsModel? FileDetails = null;
        string fileExtension = string.Empty;
        string fileContentType = string.Empty;
        if (DocumentFile != null)
        {
            if (DocumentFile.Length == 0)
                return Ok(ResponseModel<string>.Failure("Invalid file."));
           
            long maxFileSize = pdfFileSizeAllowed * 1024;
            if (DocumentFile.Length > maxFileSize)
                return Ok(ResponseModel<string>.Failure($"Invalid file. Maximum {(maxFileSize / 1024)} KB file is allowed!"));
           
            string[] allowedExtensions = UtilityClass.AllowedExtensions;
            string[] allowedMimeTypes = UtilityClass.AllowedMimeTypes;

            fileExtension = Path.GetExtension(DocumentFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));
           
            fileContentType = DocumentFile.ContentType.ToLowerInvariant();
            if (!allowedMimeTypes.Any(x => x.Equals(fileContentType)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));
            
            FileDetails = DocumentFile != null
            ? await singleFileUpload.UploadFileInFolder(DocumentFile, allowedExtensions, allowedMimeTypes, UtilityClass.EMPLOYEE_DOCS, requestData.EmployeeId, (requestData.EmployeeId + "_ACHIEVEMENT_" + DateTime.Now.Ticks.ToString() + fileExtension))
            : null;
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        EmployeeAchievementModel saveModel = new EmployeeAchievementModel()
        {
            EmployeeId = requestData.EmployeeId,
            Achievement = requestData.Achievement,
            FileUploaded = FileDetails?.FileName,
            FileExtension = fileExtension,
            FileContentType = fileContentType,
            FilePath = FileDetails?.FilePath,
            IsActive = requestData.IsActive,
            CreatedBy = LoggedInUserId,
            ModifiedBy = LoggedInUserId
        };

        await unitOfWork.EmployeeAchievementRepo.Add(saveModel);
        await unitOfWork.Save();

        return Ok(ResponseModel<long>.Success(saveModel.RowId, "Employee Achievement Details Added Successfully.", StatusCodes.Status201Created));        
    }

    [HttpPost("updateachievement")]
    public async Task<IActionResult> UpdateAchievement([FromForm] EmployeeAchievementUpdateDTO requestData, IFormFile DocumentFile = null)
    {
        if (requestData == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var record = await unitOfWork.EmployeeAchievementRepo.GetFirstOrDefault(x => x.RowId == requestData.RecordId && x.EmployeeId == requestData.EmployeeId);
        if (record == null)
            return Ok(ResponseModel<string>.NoData("No Data Found."));
       
        long pdfFileSizeAllowed = Convert.ToInt64(configuration["PDFUploadFileSize"]);

        UploadedFileDetailsModel? FileDetails = null;       
        if (DocumentFile != null)
        {
            string fileExtension = string.Empty;
            string fileContentType = string.Empty;

            if (DocumentFile.Length == 0)
                return Ok(ResponseModel<string>.Failure("Invalid file."));

            long maxFileSize = pdfFileSizeAllowed * 1024;
            if (DocumentFile.Length > maxFileSize)
                return Ok(ResponseModel<string>.Failure($"Invalid file. Maximum {(maxFileSize / 1024)} KB file is allowed!"));

            string[] allowedExtensions = UtilityClass.AllowedExtensions;
            string[] allowedMimeTypes = UtilityClass.AllowedMimeTypes;

            fileExtension = Path.GetExtension(DocumentFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            fileContentType = DocumentFile.ContentType.ToLowerInvariant();
            if (!allowedMimeTypes.Any(x => x.Equals(fileContentType)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            singleFileUpload.RemoveFile(record.FileUploaded ?? string.Empty, UtilityClass.EMPLOYEE_DOCS, "", record.EmployeeId);
            FileDetails = DocumentFile != null
           ? await singleFileUpload.UploadFileInFolder(DocumentFile, allowedExtensions, allowedMimeTypes, UtilityClass.EMPLOYEE_DOCS, requestData.EmployeeId, (requestData.EmployeeId + "_ACHIEVEMENT_" + DateTime.Now.Ticks.ToString() + fileExtension))
           : null;            
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        await unitOfWork.EmployeeAchievementRepo.Update(requestData, FileDetails, LoggedInUserId);

        return Ok(ResponseModel<string>.Success(string.Empty, "Employee Achievement Details Updated Successfully."));      
    }

    [HttpGet("achievements/{employeeid}")]
    public async Task<IActionResult> GetAllAchievements([FromRoute] string employeeid)
    {
        var allData = await unitOfWork.EmployeeAchievementRepo.GetAllAchievements(employeeid);
        if (allData == null)
            return Ok(ResponseModel<string>.NoData("No Data Found."));

        //string fileUploadPath = singleFileUpload.GetUploadPath(UtilityClass.EMPLOYEE_DOCS, "", employeeid);
        //foreach (var rec in allData)
        //    rec.FileUploaded = !string.IsNullOrEmpty(rec.FileUploaded) ? Path.Combine(fileUploadPath, rec.FileUploaded).Replace("\\", "/") : string.Empty;

        return Ok(ResponseModel<List<EmployeeAchievementDTO>?>.Success(allData, "Achievement details retrieved successfully."));       
    }

    [HttpPost("deleteachievement/{employeeid}/{recordid}")]
    public async Task<IActionResult> DeleteAchievement([FromRoute] string employeeid, [FromRoute] long recordid)
    {
        var ToDeleteRecord = await unitOfWork.EmployeeAchievementRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid && x.RowId == recordid);
        if (ToDeleteRecord == null) return Ok(ResponseModel<string>.NoData("No Data Found."));

        await unitOfWork.EmployeeAchievementRepo.Remove(ToDeleteRecord);
        await unitOfWork.Save();

        if (!string.IsNullOrEmpty(ToDeleteRecord.FileUploaded))
            singleFileUpload.RemoveFile(ToDeleteRecord.FileUploaded, UtilityClass.EMPLOYEE_DOCS, "", ToDeleteRecord.EmployeeId);

        return Ok(ResponseModel<string>.Success(string.Empty, "Achievement details deleted successfully!"));      
    }

    #endregion

    #region Employee Experience
    [HttpPost("addexperience")]
    public async Task<IActionResult> AddExperience([FromForm] EmployeeExperienceRequestDTO requestData, IFormFile DocumentFile = null)
    {
        long pdfFileSizeAllowed = Convert.ToInt64(configuration["PDFUploadFileSize"]);
        if (requestData == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        UploadedFileDetailsModel? FileDetails = null;        
        if (DocumentFile != null)
        {
            string fileExtension = string.Empty;
            string fileContentType = string.Empty;

            if (DocumentFile.Length == 0)
                return Ok(ResponseModel<string>.Failure("Invalid file."));

            long maxFileSize = pdfFileSizeAllowed * 1024;
            if (DocumentFile.Length > maxFileSize)
                return Ok(ResponseModel<string>.Failure($"Invalid file. Maximum {(maxFileSize / 1024)} KB file is allowed!"));

            string[] allowedExtensions = UtilityClass.AllowedExtensions;
            string[] allowedMimeTypes = UtilityClass.AllowedMimeTypes;

            fileExtension = Path.GetExtension(DocumentFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            fileContentType = DocumentFile.ContentType.ToLowerInvariant();
            if (!allowedMimeTypes.Any(x => x.Equals(fileContentType)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            FileDetails = DocumentFile != null
           ? await singleFileUpload.UploadFileInFolder(DocumentFile, allowedExtensions, allowedMimeTypes, UtilityClass.EMPLOYEE_DOCS, requestData.EmployeeId, (requestData.EmployeeId + "_EXPERIENCE_" + DateTime.Now.Ticks.ToString() + fileExtension))
           : null;
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        EmployeeExperienceModel saveModel = new EmployeeExperienceModel()
        {
            EmployeeId = requestData.EmployeeId,
            Experience = requestData.Experience,
            FileUploaded = FileDetails?.FileName,
            FileExtension = FileDetails? .FileExtension,
            FileContentType = FileDetails?.FileMimeType,
            FilePath = FileDetails?.FilePath,
            IsActive = requestData.IsActive,
            CreatedBy = LoggedInUserId,
            ModifiedBy = LoggedInUserId
        };

        await unitOfWork.EmployeeExperienceRepo.Add(saveModel);
        await unitOfWork.Save();

        return Ok(ResponseModel<long>.Success(saveModel.RowId, "Employee Experience Details Added Successfully.", StatusCodes.Status201Created));
    }

    [HttpPost("update_experience")]
    public async Task<IActionResult> UpdateExperience([FromForm] EmployeeExperienceUpdateDTO requestData, IFormFile DocumentFile = null)
    {
        if (requestData == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var record = await unitOfWork.EmployeeExperienceRepo.GetFirstOrDefault(x => x.RowId == requestData.RecordId && x.EmployeeId == requestData.EmployeeId);
        if (record == null) return Ok(ResponseModel<string>.NoData("No Data Found."));

        long pdfFileSizeAllowed = Convert.ToInt64(configuration["PDFUploadFileSize"]);

        UploadedFileDetailsModel? FileDetails = null;
        
        if (DocumentFile != null)
        {
            string fileExtension = string.Empty;
            string fileContentType = string.Empty;

            if (DocumentFile.Length == 0)
                return Ok(ResponseModel<string>.Failure("Invalid file."));

            long maxFileSize = pdfFileSizeAllowed * 1024;
            if (DocumentFile.Length > maxFileSize)
                return Ok(ResponseModel<string>.Failure($"Invalid file. Maximum {(maxFileSize / 1024)} KB file is allowed!"));

            string[] allowedExtensions = UtilityClass.AllowedExtensions;
            string[] allowedMimeTypes = UtilityClass.AllowedMimeTypes;

            fileExtension = Path.GetExtension(DocumentFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            fileContentType = DocumentFile.ContentType.ToLowerInvariant();
            if (!allowedMimeTypes.Any(x => x.Equals(fileContentType)))
                return Ok(ResponseModel<string>.Failure("Invalid file. Only *.JPG, *.PNG or *.PDF files are allowed!"));

            singleFileUpload.RemoveFile(record.FileUploaded ?? string.Empty, UtilityClass.EMPLOYEE_DOCS, "", record.EmployeeId);
            FileDetails = DocumentFile != null
           ? await singleFileUpload.UploadFileInFolder(DocumentFile, allowedExtensions, allowedMimeTypes, UtilityClass.EMPLOYEE_DOCS, requestData.EmployeeId, (requestData.EmployeeId + "_EXPERIENCE_" + DateTime.Now.Ticks.ToString() + fileExtension))
           : null;          
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        await unitOfWork.EmployeeExperienceRepo.Update(requestData, FileDetails, LoggedInUserId);

        return Ok(ResponseModel<string>.Success(string.Empty, "Employee Experience Details Updated Successfully."));      
    }

    [HttpGet("experience/{employeeid}")]
    public async Task<IActionResult> GetAllExperiences([FromRoute] string employeeid)
    {
        var allData = await unitOfWork.EmployeeExperienceRepo.GetAllExperiences(employeeid);
        if (allData == null)
            return Ok(ResponseModel<string>.NoData("No Data Found."));

        //string fileUploadPath = singleFileUpload.GetUploadPath(UtilityClass.EMPLOYEE_DOCS, "", employeeid);
        //foreach (var rec in allData)
        //    rec.FileUploaded = !string.IsNullOrEmpty(rec.FileUploaded) ? Path.Combine(fileUploadPath, rec.FileUploaded).Replace("\\", "/") : string.Empty;
        return Ok(ResponseModel<List<EmployeeExperienceDTO>?>.Success(allData, "Experience details retrieved successfully."));     
    }

    [HttpPost("deleteexperience/{employeeid}/{recordid}")]
    public async Task<IActionResult> DeleteExperience([FromRoute] string employeeid, [FromRoute] long recordid)
    {
        var ToDeleteRecord = await unitOfWork.EmployeeExperienceRepo.GetFirstOrDefault(x => x.EmployeeId == employeeid && x.RowId == recordid);
        if (ToDeleteRecord == null) return Ok(ResponseModel<string>.NoData("No Data Found."));

        await unitOfWork.EmployeeExperienceRepo.Remove(ToDeleteRecord);
        await unitOfWork.Save();

        if (!string.IsNullOrEmpty(ToDeleteRecord.FileUploaded))
            singleFileUpload.RemoveFile(ToDeleteRecord.FileUploaded, UtilityClass.EMPLOYEE_DOCS, "", ToDeleteRecord.EmployeeId);

        return Ok(ResponseModel<string>.Success(string.Empty, "Experience details deleted successfully!"));        
    }  
    #endregion
}
