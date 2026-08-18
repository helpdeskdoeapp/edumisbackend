using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Pagination;
using edumis.Models.Tenders;
using edumis.Models.Tenders.DTO;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Tenders;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TendersController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{    
    #region Tenders API Methods
    [HttpPost("add")]   
    public async Task<ActionResult> Add([FromForm] TenderRequestDTO requestDTO, IFormFile? file = null)
    {
        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.NoData("Unable to fetch current financial year!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            fileDetails = file != null
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.TENDERS, currentSessionData.ForSession)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var SaveObj = new TendersModel
        {
            FinancialYear = currentSessionData.ForSession,
            Title = requestDTO.Title,
            Description = requestDTO.Description,
            TenderDate = requestDTO.TenderDate,
            FileName = fileDetails?.FileName,
            FileContentType = fileDetails?.FileMimeType,
            FileExtn = fileDetails?.FileExtension,
            FilePath = fileDetails?.FilePath,
            ExpiryDate = requestDTO.ExpiryDate,
            ExpiryTime = requestDTO.ExpiryTime,
            IsValid = true,
            CreatedBy = BranchUserId,                                            
            ModifiedBy = BranchUserId
        };

        await unitOfWork.Tenders.Add(SaveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(SaveObj.RowId.ToString(), "Tender Details Saved!", StatusCodes.Status201Created));        
    }

    [HttpPost("search/{activeonly?}")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchTenders([FromBody] SearchTendersRequestDTO requestDTO, [FromRoute] bool? activeonly)
    {

        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (string.IsNullOrEmpty(requestDTO.ForSession))
        {
            var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
            if (currentSessionData != null)
                requestDTO.ForSession = currentSessionData.ForSession;
        }

        if (!requestDTO.FromDate.HasValue)
            requestDTO.FromDate = string.IsNullOrEmpty(requestDTO.ForSession)
                ? DateOnly.FromDateTime(new DateTime(1990, 1, 1))
                : DateOnly.FromDateTime(new DateTime(int.Parse(requestDTO.ForSession.Split('-')[0]), 4, 1));

        if (!requestDTO.ToDate.HasValue)
            requestDTO.ToDate = string.IsNullOrEmpty(requestDTO.ForSession)
                ? DateOnly.FromDateTime(DateTime.Today)
                : DateOnly.FromDateTime(new DateTime(int.Parse(requestDTO.ForSession.Split('-')[1]), 3, 31));

        if (requestDTO.FromDate > requestDTO.ToDate)
            return Ok(ResponseModel<string>.Failure("Invalid Date Intervals!"));
       
        if (requestDTO.PageNumber <= 0 || requestDTO.PageSize <= 0)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        var tenders = await unitOfWork.Tenders.GetAll(x =>
            x.FinancialYear == requestDTO.ForSession &&
            x.ExpiryDate >= requestDTO.FromDate &&
            x.ExpiryDate <= requestDTO.ToDate);

        if (tenders == null || !tenders.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<TendersDetailsResponseDTO>>(tenders);

        if (activeonly.HasValue)
            FinalData = FinalData.Where(x => x.IsValid == activeonly.Value).ToList();

        var sorted = FinalData.OrderBy(x => x.ExpiryDate);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<TendersDetailsResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = FinalData.Count
        };
        return Ok(ResponseModel<PaginatedResponseDTO<TendersDetailsResponseDTO>>.Success(response, "Details retrieved successfully."));        
    }

    [HttpGet("current-tenders")]
    [AllowAnonymous]
    public async Task<IActionResult> ActiveTenders()
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));               

        var tenders = await unitOfWork.Tenders.GetAll(x => x.IsValid == true &&
            x.ExpiryDate >= DateOnly.FromDateTime(DateTime.Today));

        if (tenders == null || !tenders.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<TendersDetailsResponseDTO>>(tenders);
        
        var sorted = FinalData.OrderBy(x => x.ExpiryDate);
      
        return Ok(ResponseModel<List<TendersDetailsResponseDTO>>.Success(sorted.ToList(), "Tenders retrieved successfully."));
    }

    [HttpGet("tender-by-id/{recordid}")]
    public async Task<IActionResult> GetTenderById([FromRoute] long recordid)
    {
        var tenderDetails = await unitOfWork.Tenders.GetFirstOrDefault(x => x.RowId == recordid);
        if (tenderDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = mapper.Map<TendersDetailsResponseDTO>(tenderDetails);

        return Ok(ResponseModel<TendersDetailsResponseDTO>.Success(returnData, "Details retrieved successfully."));
    }
        
    [HttpPost("update")]   
    public async Task<IActionResult> UpdateTender([FromForm] TenderUpdateRequestDTO requestDTO, IFormFile? file = null)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var tenderDetails = await unitOfWork.Tenders.GetFirstOrDefault(x => x.RowId == requestDTO.RecordId);
        if (tenderDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            if(!string.IsNullOrEmpty(tenderDetails.FileName))                          
                singleFileUpload.RemoveFile(tenderDetails.FilePath, tenderDetails.FileName);            

            fileDetails = file != null
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.TENDERS, tenderDetails.FinancialYear)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        tenderDetails.Title = requestDTO.Title;
        tenderDetails.Description = requestDTO.Description;
        tenderDetails.TenderDate = requestDTO.TenderDate;

        if (fileDetails != null)
        {
            tenderDetails.FileName = fileDetails?.FileName;
            tenderDetails.FileContentType = fileDetails?.FileMimeType;
            tenderDetails.FileExtn = fileDetails?.FileExtension;
            tenderDetails.FilePath = fileDetails?.FilePath;
        }

        tenderDetails.ExpiryDate = requestDTO.ExpiryDate;
        tenderDetails.ExpiryTime = requestDTO.ExpiryTime;
        tenderDetails.ModifiedBy = BranchUserId;
        tenderDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Tender Details Updated Successfully.", StatusCodes.Status200OK));       
    }

    [HttpPost("update-status/{recordid}/{status}")] 
    public async Task<IActionResult> UpdateTenderStatus([FromRoute] long recordid, [FromRoute] bool status)
    {       
        var tenderDetails = await unitOfWork.Tenders.GetFirstOrDefault(x => x.RowId == recordid);
        if (tenderDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;
                
        tenderDetails.IsValid = status;        
        tenderDetails.ModifiedBy = BranchUserId;
        tenderDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Tender Activated." : "Tender Deactivated.", StatusCodes.Status200OK));        
    }
    #endregion
}
