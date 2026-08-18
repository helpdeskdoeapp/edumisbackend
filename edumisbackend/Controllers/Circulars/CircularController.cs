using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Circulars;
using edumis.Models.Circulars.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Circulars;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CircularController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{
    #region Circulars API Methods
    [HttpPost("add")]        
    public async Task<IActionResult> AddCircular([FromForm] CircularRequestDataDTO requestDTO, IFormFile? file = null)
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
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.CIRCULARS, currentSessionData.ForSession)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));           
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var saveObj = new CircularModel
        {
            FinancialYear = currentSessionData.ForSession,
            CircularDate = requestDTO.CircularDate,
            Title = requestDTO.Title,
            Description = requestDTO.Description,
            Type = requestDTO.Type,
            FileName = fileDetails?.FileName,
            FileContentType = fileDetails?.FileMimeType,
            FileExtn = fileDetails?.FileExtension,
            FilePath = fileDetails?.FilePath,
            IsValid = true,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };

        await unitOfWork.Circular.Add(saveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(saveObj.RowId.ToString(), "Circular Details Saved!", StatusCodes.Status201Created));        
    }

    [HttpPost("search/{activeonly?}")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchCirculars([FromBody] SearchCircularsRequestDTO requestDTO, [FromRoute] bool? activeonly)
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
       
        if(requestDTO.PageNumber <= 0 || requestDTO.PageSize <= 0)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));
        
        var circulars = await unitOfWork.Circular.GetAll(x =>
            x.FinancialYear == requestDTO.ForSession &&
            x.CircularDate >= requestDTO.FromDate &&
            x.CircularDate <= requestDTO.ToDate);

        if (circulars == null || !circulars.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<CircularsDetailResponseDTO>>(circulars);

        if (activeonly.HasValue)
            FinalData = FinalData.Where(x => x.IsValid == activeonly.Value).ToList();

        var sorted = FinalData.OrderByDescending(x => x.CircularDate);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<CircularsDetailResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = FinalData.Count
        };
        return Ok(ResponseModel<PaginatedResponseDTO<CircularsDetailResponseDTO>>.Success(response, "Details retrieved successfully."));      
    }

    [HttpGet("circular-by-id/{recordid}")]
    public async Task<IActionResult> GetCircularById([FromRoute] long recordid)
    {
        var CircularTypes = await unitOfWork.CodeValues.GetAll(x=>x.Code == GlobalConstants.CircularTypes);

        var tenderDetails = await unitOfWork.Circular.GetFirstOrDefault(x => x.RowId == recordid);
        if (tenderDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var mappedData = mapper.Map<CircularsDetailResponseDTO>(tenderDetails);
        var codeDetails = await unitOfWork.CodeValues.GetFirstOrDefault(x => x.CodeValue == tenderDetails.Type);
        mappedData.TypeDesc = codeDetails.CodeValDescription;

        return Ok(ResponseModel<CircularsDetailResponseDTO>.Success(mappedData, "Details retrieved successfully."));
    }

    [HttpPost("update")]       
    public async Task<IActionResult> Update([FromForm] CircularUpdateRequestDTO requestDTO, IFormFile? file = null)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var circularDetails = await unitOfWork.Circular.GetFirstOrDefault(r => r.RowId == requestDTO.RecordId);
        if (circularDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            if (!string.IsNullOrEmpty(circularDetails.FileName))
                singleFileUpload.RemoveFile(circularDetails.FilePath, circularDetails.FileName);

            fileDetails = file != null
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.TENDERS, circularDetails.FinancialYear)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        circularDetails.Title = requestDTO.Title;
        circularDetails.Description = requestDTO.Description;
        circularDetails.CircularDate = requestDTO.CircularDate;
        circularDetails.Type = requestDTO.Type;

        if (fileDetails != null)
        {
            circularDetails.FileName = fileDetails?.FileName;
            circularDetails.FileContentType = fileDetails?.FileMimeType;
            circularDetails.FileExtn = fileDetails?.FileExtension;
            circularDetails.FilePath = fileDetails?.FilePath;
        }
        
        circularDetails.ModifiedBy = BranchUserId;
        circularDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Circular Details Updated Successfully.", StatusCodes.Status200OK));       
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] long recordid, [FromRoute] bool status)
    {
        var circularDetails = await unitOfWork.Circular.GetFirstOrDefault(x => x.RowId == recordid);
        if (circularDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        circularDetails.IsValid = status;
        circularDetails.ModifiedBy = BranchUserId;
        circularDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Circular Activated." : "Circular Deactivated.", StatusCodes.Status200OK));       
    }
    #endregion
}
