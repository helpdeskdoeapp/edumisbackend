using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models.Alumni.Members;
using edumis.Models.Alumni.Members.DTO;
using edumis.Models.Alumni.UserAccounts;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Alumni;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AlumniController(IUnitOfWork unitOfWork, IConfiguration configuration) : ControllerBase
{
    #region registration API
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AlumniRegistrationRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid registration request!"));
        
        if(await unitOfWork.AlumniDetailsRepo.Exists(x=>x.EmailID.ToLower() == requestDTO.EmailID.ToLower()))
            return Ok(ResponseModel<string>.Failure("An alumni with the same Email Id already exists!", StatusCodes.Status409Conflict));

        //if (!string.IsNullOrEmpty(requestDTO.DOERegistrationId) &&
        //   await unitOfWork.AlumniDetailsRepo.Exists(x => x.DOERegistrationId == requestDTO.DOERegistrationId))
        //    return Ok(ResponseModel<string>.Failure("An alumni with the same DOE registration id already exists!", StatusCodes.Status409Conflict));

        if(requestDTO.Password.Length < 8)
            return Ok(ResponseModel<string>.Failure("Password must contain at least 8 characters."));

        if(!Utilities.IsValidPassword(requestDTO.Password))
            return Ok(ResponseModel<string>.Failure("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."));

        if (!string.IsNullOrEmpty(requestDTO.MobileNo) && requestDTO.MobileNo.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile No. It must contain exactly 10 digits."));

        var saveObj = new AlumniDetailsModel(
            new Guid(),
            requestDTO.DOERegistrationId,
            requestDTO.Salutation,
            requestDTO.FirstName,
            requestDTO.LastName,
            requestDTO.MiddleName,
            requestDTO.DOB,
            requestDTO.Gender,
            requestDTO.RegistrationYear,
            requestDTO.ExitYear,
            requestDTO.BranchId,
            requestDTO.BranchNotInList,
            requestDTO.OtherBranchName,
            requestDTO.EmailID,
            requestDTO.AlternateEmailId,
            requestDTO.MobileNo,
            requestDTO.CurrentOrganization,
            requestDTO.CurrentDesignation,
            requestDTO.CurrentResidence,
            requestDTO.ResidenceContactNo,
            requestDTO.WorkContactNo,
            requestDTO.CurrentResidenceCity,
            requestDTO.CurrentProfession,
            requestDTO.OtherProfession,
            requestDTO.IsResidentOfDelhi,
            true
        );

        await unitOfWork.AlumniDetailsRepo.Add(saveObj);

        var alumniShareInfoObj = new AlumniInformationShareModel(saveObj.AlumniId, true, true, true, true, true, true, true, true, true);
        await unitOfWork.AlumniInformationShareRepo.Add(alumniShareInfoObj);

        var userObj = new AlumniLoginModel(saveObj.AlumniId, requestDTO.EmailID, Utilities.HashPassword(requestDTO.Password));
        await unitOfWork.AlumniLoginRepo.Add(userObj);

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Registration successful!", StatusCodes.Status200OK));        
    }

    [HttpPost("register-branch-alumni")]
    public async Task<IActionResult> RegisterBranchAlumni([FromBody] SchoolAlumniRegistrationRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid registration request!"));

        if(requestDTO.CreateLogin && string.IsNullOrEmpty(requestDTO.EmailID))
            return Ok(ResponseModel<string>.Failure("Email id is required for user creation.", StatusCodes.Status406NotAcceptable));

        if (!string.IsNullOrEmpty(requestDTO.EmailID) && 
            await unitOfWork.AlumniDetailsRepo.Exists(x => x.EmailID.ToLower() == requestDTO.EmailID.ToLower()))
            return Ok(ResponseModel<string>.Failure("An alumni with the same Email Id already exists!", StatusCodes.Status409Conflict));

        if (!string.IsNullOrEmpty(requestDTO.DOERegistrationId) &&
           await unitOfWork.AlumniDetailsRepo.Exists(x => x.DOERegistrationId == requestDTO.DOERegistrationId))
            return Ok(ResponseModel<string>.Failure("An alumni with the same DOE registration id already exists!", StatusCodes.Status409Conflict));

        if (!string.IsNullOrEmpty(requestDTO.MobileNo) && requestDTO.MobileNo.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile No. It must contain exactly 10 digits."));

        var saveObj = new AlumniDetailsModel(
            new Guid(),
            requestDTO.DOERegistrationId,
            requestDTO.Salutation,
            requestDTO.FirstName,
            requestDTO.LastName,
            requestDTO.MiddleName,
            requestDTO.DOB,
            requestDTO.Gender,
            requestDTO.RegistrationYear,
            requestDTO.ExitYear,
            requestDTO.BranchId,
            false,
            string.Empty,
            requestDTO.EmailID,
            string.Empty,
            requestDTO.MobileNo,
            requestDTO.CurrentOrganization,
            requestDTO.CurrentDesignation,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            requestDTO.CurrentProfession,
            requestDTO.OtherProfession,
            requestDTO.IsResidentOfDelhi,
            true
        );

        await unitOfWork.AlumniDetailsRepo.Add(saveObj);

        var alumniShareInfoObj = new AlumniInformationShareModel(saveObj.AlumniId, true, true, true, true, true, true, true, true, true);
        await unitOfWork.AlumniInformationShareRepo.Add(alumniShareInfoObj);

        if (requestDTO.CreateLogin)
        {
            var userObj = new AlumniLoginModel(saveObj.AlumniId, requestDTO.EmailID,
                Utilities.HashPassword($"{(requestDTO.FirstName?.Length >= 4
                    ? requestDTO.FirstName[..4].ToUpper()
                    : requestDTO.FirstName?.ToUpper())}{requestDTO.DOB:ddMM}"
            )); 
            await unitOfWork.AlumniLoginRepo.Add(userObj);
        }

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Registration successful!", StatusCodes.Status200OK));
    }
    #endregion

    #region Update APIs
    [HttpPatch("update-status/{alumniId}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] string alumniId, [FromQuery] bool status)
    {
        if (!Guid.TryParse(alumniId, out var alumniGuid))        
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));
        
        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == new Guid(alumniId));
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetActivationStatus(status);  
        
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, status ? "Alumni activated successfully!" : "Alumni deactivated successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-profile-image")]
    public async Task<IActionResult> UpdateProfileImage(IFormFile file)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;
             
        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        if (file == null || file.Length == 0)
            return Ok(ResponseModel<string>.Failure("Invalid file upload request", StatusCodes.Status400BadRequest));
                
        long maxFileSize = Convert.ToInt32(configuration["UserProfileFileSize"]) * 1024;
        if (file.Length > maxFileSize) 
            return Ok(ResponseModel<string>.Failure("Profile photo file size exceeds the 50KB limit!", StatusCodes.Status400BadRequest));
        
        string[] allowedExtensions = UtilityClass.AllowedImageExtensions;
        string[] allowedMimeTypes = UtilityClass.AllowedImageMimeTypes;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Any(x => x.Equals(fileExtension)))
            return Ok(ResponseModel<string>.Failure("Invalid image file. Only *.JPG or &.PNG files are allowed!"));

        if (!allowedMimeTypes.Any(x => x.Equals(file.ContentType.ToLowerInvariant())))
            return Ok(ResponseModel<string>.Failure("Invalid image file. Only *.JPG or &.PNG files are allowed!"));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
                      
            alumni.SetProfileImage(memoryStream.ToArray(), fileExtension, file.ContentType, alumniGuid.ToString());
            await unitOfWork.Save();      

            return Ok(ResponseModel<string>.Success(string.Empty, "Profile photo updated."));
        }
    }

    [HttpPost("update-enrollment-info")]
    public async Task<IActionResult> UpdateEnrollmentDetails([FromBody] AlumniEnrollmentUpdateRequestDTO requestDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.UpdateEnrollmentDetails(
           requestDTO.DOERegistrationId,
           requestDTO.RegistrationYear,
           requestDTO.ExitYear,
           requestDTO.BranchId,
           requestDTO.BranchId == "9999999" ? true : false,
           requestDTO.OtherBranchName,
           alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Details updated successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-personal-info")]
    public async Task<IActionResult> UpdatePersonalInfoDetails([FromBody] AlumniPersonalInfoUpdateRequestDTO requestDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.UpdatePersonalInfoDetails(
            requestDTO.Salutation,
            requestDTO.FirstName,
            requestDTO.LastName,
            requestDTO.MiddleName,
            requestDTO.DOB,
            requestDTO.Gender,
            requestDTO.EmailID,
            requestDTO.MobileNo,
            alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Details updated successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-professional-details")]
    public async Task<IActionResult> UpdateProfessionalDetails([FromBody] AlumniProfessionalDetailsUpdateRequestDTO requestDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.UpdateProfessionalDetails(
            requestDTO.CurrentOrganization, 
            requestDTO.CurrentDesignation, 
            requestDTO.CurrentProfession,
            requestDTO.OtherProfession, 
            requestDTO.WorkContactNo,
            alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Details updated successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-contact-details")]
    public async Task<IActionResult> UpdateContactDetails([FromBody] AlumniContactDetailsUpdateRequestDTO requestDTO)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.UpdateContactDetails(             
            requestDTO.AlternateEmailId, 
            requestDTO.ResidenceContactNo,           
            requestDTO.IsResidentOfDelhi, 
            requestDTO.CurrentResidence, 
            requestDTO.CurrentResidenceCity,
            alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Details updated successfully!", StatusCodes.Status200OK));
    }
    #endregion

    #region Update Permission APIs
    [HttpPost("allow-email-info/{status}")]
    public async Task<IActionResult> AllowEmail([FromRoute] bool status)
    {        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetEmailPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-mobile-info/{status}")]
    public async Task<IActionResult> AllowMobile([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetMobileNoPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-current-org-info/{status}")]
    public async Task<IActionResult> AllowCurrentOrganisationInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetCurrentOrganisationPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-current-desig-info/{status}")]
    public async Task<IActionResult> AllowCurrentDesignationInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetCurrentDesignationPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-current-residence-info/{status}")]
    public async Task<IActionResult> AllowCurrentResidenceInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetCurrentResidencePermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-residence-contact-info/{status}")]
    public async Task<IActionResult> AllowResidenceContactNoInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetResidenceContactNoPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-work-contact-info/{status}")]
    public async Task<IActionResult> AllowWorkContactNoInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetWorkContactNoPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-current-residence-city-info/{status}")]
    public async Task<IActionResult> AllowCurrentResidenceCityInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetCurrentResidenceCityPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("allow-current-profession-info/{status}")]
    public async Task<IActionResult> AllowCurrentProfessionInfo([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniInformationShareRepo.GetFirstOrDefault(x => x.AlumniID == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni not found", StatusCodes.Status404NotFound));

        alumni.SetCurrentProfessionPermissionStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Pemission updated.", StatusCodes.Status200OK));
    }

    [HttpPost("show-on-homepage/{status}")]
    public async Task<IActionResult> ShowOnHomePage([FromRoute] bool status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var alumni = await unitOfWork.AlumniDetailsRepo.GetFirstOrDefault(x => x.AlumniId == alumniGuid);
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Alumni details not found", StatusCodes.Status404NotFound));

        alumni.SHowOnHomePageStatus(status, alumniGuid.ToString());
        await unitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Status updated.", StatusCodes.Status200OK));
    }
    #endregion

    #region Get Details API
    [HttpGet("details/{alumniId}")]
    public async Task<IActionResult> AlumniDetails([FromRoute] string alumniId)
    {
        if (!Guid.TryParse(alumniId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var returnData = await unitOfWork.AlumniDetailsRepo.GetDetails(alumniGuid);
        if (returnData == null)
            return Ok(ResponseModel<string>.NoData("Alumni details not found!", StatusCodes.Status404NotFound));

        return Ok(ResponseModel<AlumniDetailsDTO>.Success(returnData, "Alumni details fetched successfully", StatusCodes.Status200OK));
    }

    [HttpGet("collage-profiles/{records?}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCollageProfile(int? records)
    {
        var returnData = await unitOfWork.AlumniDetailsRepo.GetCollageAlumni(records ?? 10);
        if (returnData == null)
            return Ok(ResponseModel<string>.NoData("No Data Found!", StatusCodes.Status404NotFound));
        return Ok(ResponseModel<List<SelectedAlumniCollageDTO>>.Success(returnData, "Data fetched successfully", StatusCodes.Status200OK));
    }
    #endregion

    #region Search APIs
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] AlumniSearchRequestDTO requestDTO)
    {
       var AllAlumniList = await unitOfWork.AlumniDetailsRepo.GetAll();

        if (AllAlumniList == null || AllAlumniList.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No alumni data available.", StatusCodes.Status404NotFound));

        var returnData = await unitOfWork.AlumniDetailsRepo.Search(requestDTO);

        if (returnData == null || returnData.Count == 0)
            return Ok(ResponseModel<string>.NoData("No alumni found matching the search criteria.", StatusCodes.Status404NotFound));

        var sorted = requestDTO.SortBy?.ToLower() switch
        {
            "name" => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.FirstName) : returnData.OrderBy(u => u.FirstName),
            "gender" => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.GenderTitle) : returnData.OrderBy(u => u.GenderTitle),
            "registrationyear" => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.RegistrationYear) : returnData.OrderBy(u => u.RegistrationYear),
            "branch" => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.BranchName) : returnData.OrderBy(u => u.BranchName),
            "dob" => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.DOB) : returnData.OrderBy(u => u.DOB),
            _ => requestDTO.SortDescending ? returnData.OrderByDescending(u => u.FirstName) : returnData.OrderBy(u => u.FirstName)
        };

        var paginated = sorted
            .Skip(((requestDTO.PageNumber??1) - 1) * (requestDTO.PageSize??10))
            .Take(requestDTO.PageSize??10)
            .ToList();

        var response = new PaginatedResponseDTO<AlumniSearchResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber ?? 1,
            PageSize = requestDTO.PageSize ?? 10,
            TotalCount = returnData.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<AlumniSearchResponseDTO>>.Success(response, "Alumni search results fetched successfully."));
    }

    [HttpPost("find/{pageno?}/{pagesize?}")]
    public async Task<IActionResult> FindAlumni([FromBody] string searchText, [FromRoute] int? pageno = 1, [FromRoute] int? pagesize = 10)
    {
        if (string.IsNullOrEmpty(searchText))
            return Ok(ResponseModel<string>.Failure("Invalid search text!", StatusCodes.Status403Forbidden));

        var allAlumniList = await unitOfWork.AlumniDetailsRepo.Search(searchText);
        if (allAlumniList == null || allAlumniList.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No alumni found matching the provided text.", StatusCodes.Status404NotFound));

        var searchResult = allAlumniList.Where(x =>
            x.FirstName.ToLower().Contains(searchText.ToLower()) ||
            x.LastName.ToLower().Contains(searchText.ToLower()) ||
            x.EmailID.ToLower().Contains(searchText.ToLower()) ||
            x.BranchName.ToLower().Contains(searchText.ToLower()) ||
            x.CurrentProfession?.ToLower().Contains(searchText.ToLower()) == true
        ).ToList();

        if (searchResult == null || searchResult.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No alumni found matching the provided text.", StatusCodes.Status404NotFound));

        var sorted = searchResult.OrderBy(u => u.FirstName);

        var paginated = sorted
            .Skip(((pageno ?? 1) - 1) * (pagesize ?? 10))
            .Take(pagesize ?? 10)
            .ToList();

        var response = new PaginatedResponseDTO<AlumniSearchResponseDTO>
        {
            Items = paginated,
            PageNumber = pageno ?? 1,
            PageSize = pagesize ?? 10,
            TotalCount = searchResult.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<AlumniSearchResponseDTO>>.Success(response, "Alumni search results fetched successfully."));

    }
    #endregion
       
}
