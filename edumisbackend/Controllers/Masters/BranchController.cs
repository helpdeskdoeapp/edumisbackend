using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Communication;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using edumis.Models.MISC;
using edumis.Models.MISC.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Masters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BranchController(IUnitOfWork UnitOfWork, IConfiguration configuration, SingleFileUpload singleFileUpload, IMapper mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] BranchRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if(await UnitOfWork.BranchRepo.Exists(x => x.BranchId == requestDTO.BranchId))
            return Ok(ResponseModel<string>.Failure($"Branch ID [{requestDTO.BranchId}] already exists."));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var SaveObj = new BranchesModel()
        {
            BranchId = requestDTO.BranchId,
            BranchName = requestDTO.BranchName,
            Address = requestDTO.Address,
            BranchType = requestDTO.BranchType,
            BuildingId = requestDTO.BuildingId,
            ContactNo = requestDTO.ContactNo,
            DistrictId = requestDTO.DistrictId,
            EmailId = requestDTO.EmailId,
            InchargeId = requestDTO.InchargeId,
            ZoneId = requestDTO.ZoneId,
            ParentBranchId = requestDTO.ParentBranchId,
            IsActive = true,
            CreatedBy = LoggedInUserId,
            ModifiedBy = LoggedInUserId
        };

        await UnitOfWork.BranchRepo.Add(SaveObj);
        await UnitOfWork.Save();
        return Ok(ResponseModel<bool>.Success(true, "Branch details saved successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-status/{branchid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] string branchid, [FromRoute] bool status)
    {
        var branchDetails = await UnitOfWork.BranchRepo.GetFirstOrDefault(x => x.BranchId == branchid);

        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData($"Branchdetails not found."));


        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        branchDetails.IsActive = status;
        branchDetails.ModifiedBy = LoggedInUserId;
        branchDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, 
            status ? "Branch activated successfully!" : "Branch de-activated successfully!", 
            StatusCodes.Status200OK));
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] BranchRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var branchDetails = await UnitOfWork.BranchRepo.GetFirstOrDefault(x => x.BranchId == requestDTO.BranchId);

        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData($"Branchdetails not found."));


        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        branchDetails.Address = requestDTO.Address;
        branchDetails.BranchName = requestDTO.BranchName;
        branchDetails.ParentBranchId = requestDTO.ParentBranchId;
        branchDetails.BranchType = requestDTO.BranchType;
        branchDetails.BuildingId = requestDTO.BuildingId;
        branchDetails.ContactNo = requestDTO.ContactNo;
        branchDetails.DistrictId = requestDTO.DistrictId;
        branchDetails.EmailId = requestDTO.EmailId;
        branchDetails.InchargeId = requestDTO.InchargeId;
        branchDetails.ZoneId = requestDTO.ZoneId;
        branchDetails.ModifiedBy = LoggedInUserId;
        branchDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Branch details updated successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchBranches([FromBody] SearchBranchRequestDTO requestDTO)//<List<BranchDetailsDTO>>
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var searchData = await UnitOfWork.BranchRepo.GetBranches();
       
        if (searchData == null || !searchData.Any())
            return Ok(ResponseModel<string>.NoData("No record found!"));

        if (requestDTO.DistrictId.HasValue && requestDTO.DistrictId > 0)
            searchData = searchData.Where(x => x.DistrictId == requestDTO.DistrictId).ToList();

        if (requestDTO.ZoneId.HasValue && requestDTO.ZoneId > 0)
            searchData = searchData.Where(x => x.ZoneId == requestDTO.ZoneId).ToList();

        if (requestDTO.BranchType.HasValue && requestDTO.BranchType > 0)
            searchData = searchData.Where(x => x.BranchType == requestDTO.BranchType).ToList();

        if (requestDTO.Status.HasValue)
            searchData = searchData.Where(x => x.IsActive == requestDTO.Status).ToList();

        if (!string.IsNullOrEmpty(requestDTO.BuildingId))
            searchData = searchData.Where(x => x.BuildingId == requestDTO.BuildingId).ToList();

        if (!string.IsNullOrEmpty(requestDTO.BranchId))
            searchData = searchData.Where(x => x.BranchId == requestDTO.BranchId).ToList();

        if (!searchData.Any())
            return Ok(ResponseModel<string>.NoData("No record found!"));

        var sortedData = searchData.OrderBy(x => x.BranchId).ToList();
        var paginated = sortedData
             .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
             .Take(requestDTO.PageSize)
             .ToList();

        var response = new PaginatedResponseDTO<BranchDetailsDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = searchData.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<BranchDetailsDTO>>.Success(response, "Branch details retrieved successfully"));
    }

    [HttpGet("branch-details/{branchid}")]
    public async Task<IActionResult> GetBranchDetails([FromRoute] string branchid)
    {
        if (string.IsNullOrEmpty(branchid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var branchDetails = await UnitOfWork.BranchRepo.GetDetails(branchid);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData("Branch details not found!"));

        return Ok(ResponseModel<BranchDetailsDTO>.Success(branchDetails, "Branch details retrieved successfully"));
    }

    [AllowAnonymous]
    [HttpGet("all-branches")]
    public async Task<IActionResult> GetAllBranches()
    {
        var branchDetails = await UnitOfWork.BranchRepo.GetAll(x => x.IsActive == true);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = branchDetails.Select(x => new BranchesNamesDTO
        {
            BranchId = x.BranchId,
            BranchName = x.BranchName,
            BranchType = x.BranchType
        }).ToList();

        return Ok(ResponseModel<List<BranchesNamesDTO>>.Success(returnData.OrderBy(x=>x.BranchId).ToList(), "Branches retrieved successfully"));
    }

    [AllowAnonymous]
    [HttpPost("send-verification-sms/{branchid}")]
    public async Task<IActionResult> SendVerificationSMS([FromRoute] string branchid)
    {
        if (string.IsNullOrEmpty(branchid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var branchDetails = await UnitOfWork.BranchRepo.GetFirstOrDefault(x => x.BranchId == branchid && x.IsActive == true);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData("Branch not found!"));

        if (await UnitOfWork.SwachhBharatImagesRepo.Exists(x => x.BranchId == branchid && x.ForDate == DateOnly.FromDateTime(DateTime.Today)))
            return Ok(ResponseModel<string>.Failure("Photos already verified & uploaded for the current date!"));        

        if (string.IsNullOrEmpty(branchDetails.ContactNo) || branchDetails.ContactNo.Length < 10)
            return Ok(ResponseModel<string>.Failure("Branch contact number is not valid."));

        var SMSSettings = await UnitOfWork.SMSSettingsRepo.GetFirstOrDefault(x => x.IsValid);
        if (SMSSettings == null)
            return Ok(ResponseModel<string>.NoData("SMS Settings Not Found!"));
       
        var smsTemplate = await UnitOfWork.SMSTemplatesRepo.GetFirstOrDefault(x => x.SMSType == (int)SMS_PURPOSE.OTP_SMS && x.IsValid == true);
        if (smsTemplate == null)
            return Ok(ResponseModel<string>.NoData("SMS Template Not Found!"));
        
        Random rnd = new Random();
        long OTPText = rnd.Next(100001, 999999);
        string Message = "OTP : " + OTPText.ToString() + " --Directorate of Education";
        var ReturnSMSStatus = edumis.Common.SMS.sendOTPMSG(SMSSettings.UserID, SMSSettings.Password, SMSSettings.SenderId, branchDetails.ContactNo, Message, SMSSettings.SecureKey, smsTemplate.TemplateId, SMSSettings.SMSURL);

        if (!ReturnSMSStatus.Contains("402"))//402,MsgID = 070920211631027377372edudel-Am (Success Message returned)
            return Ok(ResponseModel<string>.Failure("Failed To Send SMS!"));       
        else
        {
            OTPSentModel modelToSave = new OTPSentModel()
            {
                SentTo = branchDetails.ContactNo,
                Purpose = "VERIFY_MOBILE",
                OTP = OTPText.ToString(),
                ValidUpTo = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["OTPExpiryTime"].ToString()))
            };
            await UnitOfWork.OTPSentRepo.Add(modelToSave);
            await UnitOfWork.Save();

            return Ok(ResponseModel<bool>.Success(true, $"OTP sent to the mobile number xxxxxxx{branchDetails.ContactNo.Substring(7)}. OTP is Valid for {configuration["OTPExpiryTime"].ToString()} minutes only."));          
        }        
    }

    [HttpPost("verify-submit-images/{otp}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifiyBranchOTPText([FromForm] string branchid, [FromRoute] string otp,
        [FromForm(Name = "oldFiles")] ICollection<IFormFile> oldFiles, [FromForm(Name = "currentFiles")] ICollection<IFormFile> currentFiles)
    {
        if (string.IsNullOrEmpty(branchid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if(await UnitOfWork.SwachhBharatImagesRepo.Exists(x=>x.BranchId ==  branchid && x.ForDate == DateOnly.FromDateTime(DateTime.Today)))
            return Ok(ResponseModel<string>.Failure("Photos already uploaded for the current date!"));

        if (oldFiles == null || !oldFiles.Any())        
            return Ok(ResponseModel<string>.Failure("Before images files are required!"));

        if (currentFiles == null || !currentFiles.Any())
            return Ok(ResponseModel<string>.Failure("After images files are required!"));

        var branchDetails = await UnitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
            x.BranchId == branchid &&
            x.IsValid == true);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.Failure("Unauthorized!", StatusCodes.Status401Unauthorized));

        if (otp.Length != 6)
            return Ok(ResponseModel<string>.Failure("Invalid OTP!"));
     
        var LastSentOTP = await UnitOfWork.OTPSentRepo.GetFirstOrDefaultByOrder(x => x.SentDate, x => x.SentTo == branchDetails.MobileNo && x.Purpose == "VERIFY_MOBILE", true);
        if (LastSentOTP == null)
            return Ok(ResponseModel<string>.Failure("Failed to verify OTP!"));
       
        if (LastSentOTP.ValidUpTo.Subtract(DateTime.Now).Minutes < 0)
            return Ok(ResponseModel<string>.Failure("OTP Expired!"));
     
        if (LastSentOTP.OTP != otp)
            return Ok(ResponseModel<string>.Failure("OTP Mismatch!"));

        var currentSessionData = await UnitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return NotFound(new ResponseModel { Success = false, Message = "Unable to verify current academic session!", ReturnCode = StatusCodes.Status204NoContent.ToString() });

        List<SwachhBharatImagesModel> swachhBharatImagesList = new List<SwachhBharatImagesModel>();

        string[] allowedExtensions = Constants.AllowedImageExtensions;
        string[] allowedMimeTypes = Constants.AllowedImageMimeTypes;

        foreach (var file in oldFiles)
        {
            if (file.Length > 0)
            {
                var fileDetails = file != null
                ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.SWACHH_BHARAT, currentSessionData.ForSession, branchid)
                : null;

                var imageUploaded = new SwachhBharatImagesModel
                {
                    BranchId = branchid,
                    ForDate = DateOnly.FromDateTime(DateTime.Today),
                    ImageUrl = fileDetails?.FilePath ?? string.Empty,
                    ImageName = fileDetails?.FileName ?? string.Empty,
                    ImageContentType = fileDetails?.FileMimeType ?? string.Empty,
                    ImageFileExtn = fileDetails?.FileExtension ?? string.Empty,
                    IsCurrent = false,
                    CreatedBy = branchid,
                    ModifiedBy = branchid
                };
                swachhBharatImagesList.Add(imageUploaded);
            }
        }

        foreach (var file in currentFiles)
        {
            if (file.Length > 0)
            {
                var fileDetails = file != null
                ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.SWACHH_BHARAT, currentSessionData.ForSession, branchid)
                : null;

                var imageUploaded = new SwachhBharatImagesModel
                {
                    BranchId = branchid,
                    ForDate = DateOnly.FromDateTime(DateTime.Today),
                    ImageUrl = fileDetails?.FilePath ?? string.Empty,
                    ImageName = fileDetails?.FileName ?? string.Empty,
                    ImageContentType = fileDetails?.FileMimeType ?? string.Empty,
                    ImageFileExtn = fileDetails?.FileExtension ?? string.Empty,
                    IsCurrent = true,
                    CreatedBy = branchid,
                    ModifiedBy = branchid
                };
                swachhBharatImagesList.Add(imageUploaded);
            }
        }

        await UnitOfWork.SwachhBharatImagesRepo.AddRange(swachhBharatImagesList);
        await UnitOfWork.Save();

        return Ok(ResponseModel<string>.Success(string.Empty,"Details submitted successfully."));
    }

    [AllowAnonymous]
    [HttpGet("swachh-bharat-images/{branchid}")]
    public async Task<IActionResult> GetSwachhBharatImages([FromRoute] string branchid)
    {
        var branchDetails = await UnitOfWork.BranchRepo.GetFirstOrDefault(x => x.BranchId == branchid && x.IsActive == true);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData("No branch details found!"));

        var imagesUploaded = await UnitOfWork.SwachhBharatImagesRepo.GetAll(x => x.BranchId == branchid);
        if (imagesUploaded == null || imagesUploaded.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No photo/image found!"));
              
        var returnData = mapper.Map<List<SwachhBharatImagesResponseDTO>>(imagesUploaded);

        return Ok(ResponseModel<List<SwachhBharatImagesResponseDTO>>.Success(returnData, "Branch/School photos retrieved successfully."));
    }

    [AllowAnonymous]
    [HttpGet("all-schools-list/{activeonly?}")]
    public async Task<IActionResult> GetSchoolsList([FromRoute] bool? activeonly)
    {
        var branchDetails = (activeonly.HasValue && activeonly == true) ? 
            await UnitOfWork.BranchRepo.GetAll(x => x.IsActive == true) : 
            await UnitOfWork.BranchRepo.GetAll();
        if (branchDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = branchDetails.Select(x => new BranchesNamesDTO
        {
            BranchId = x.BranchId,
            BranchName = x.BranchName
        }).ToList();

        return Ok(ResponseModel<List<BranchesNamesDTO>>.Success(returnData.OrderBy(x => x.BranchId).ToList(), "Data retrieved successfully"));
    }
}
