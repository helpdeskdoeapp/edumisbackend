using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Events;
using edumis.Models.Events.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Events;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{   
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm] EventRequestDTO requestDTO, IFormFile? file = null)
    {
        if (!string.IsNullOrEmpty(requestDTO.VideoLink) && (!Uri.TryCreate(requestDTO.VideoLink, UriKind.Absolute, out var vUri) ||
            (vUri.Scheme != Uri.UriSchemeHttp && vUri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid Video URL!"));
       
        if (!string.IsNullOrEmpty(requestDTO.ExternalLink) && (!Uri.TryCreate(requestDTO.ExternalLink, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid External URL!"));

        if(requestDTO.StartDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Event cannot be scheduled in the past!"));

        if (requestDTO.EndDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Invalid event end date!"));

        if (requestDTO.StartDate > requestDTO.EndDate)
            return Ok(ResponseModel<string>.Failure("Invalid Date Intervals!"));
        if (requestDTO.StartTime > requestDTO.EndTime)
            return Ok(ResponseModel<string>.Failure("Invalid Time Intervals!"));

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.NoData("Unable to fetch current financial year!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            fileDetails = file != null
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.EVENTS, currentSessionData.ForSession)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var saveObj = new EventsModel
        {
            FinancialYear = currentSessionData.ForSession,
            Category = requestDTO.Category,
            StartDate = requestDTO.StartDate,
            EndDate = requestDTO.EndDate,
            StartTime = requestDTO.StartTime,
            EndTime = requestDTO.EndTime,
            Title = requestDTO.Title,
            Description = requestDTO.Description,  
            BranchId = requestDTO.BranchId,
            ExternalLink = requestDTO.ExternalLink,
            VideoLink = requestDTO.VideoLink,
            Venue = requestDTO.Venue,
            OrganizedBy = requestDTO.OrganizedBy,
            BannerFileName = fileDetails?.FileName,
            BannerFileContentType = fileDetails?.FileMimeType,
            BannerFileExtn = fileDetails?.FileExtension,
            BannerFilePath = fileDetails?.FilePath,
            IsValid = true,
            AlumniEvent = requestDTO.AlumniEvent,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };

        await unitOfWork.EventsRepo.Add(saveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(saveObj.RowId.ToString(), "Event Details Saved!", StatusCodes.Status201Created));        
    }

    [HttpPost("search/{activeonly?}")]   
    public async Task<IActionResult> SearchEvents([FromBody] SearchEventsRequestDTO requestDTO, [FromRoute] bool? activeonly)
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

        var eventsList = await unitOfWork.EventsRepo.GetAllEvents(requestDTO);

        if (eventsList == null || !eventsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        if (requestDTO.Category.HasValue && requestDTO.Category.Value != 0)
            eventsList = eventsList.Where(x => x.Category == requestDTO.Category.Value);

        if (eventsList == null || !eventsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

       // var FinalData = mapper.Map<List<EventResponseDTO>>(eventsList);

        if (activeonly.HasValue)
            eventsList = eventsList.Where(x => x.IsValid == activeonly.Value).ToList();

        var sorted = eventsList.OrderBy(x => x.StartDate);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<EventResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = eventsList.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<EventResponseDTO>>.Success(response, "Details retrieved successfully."));       
    }

    [HttpGet("active-events/{foralumni?}")]
    [AllowAnonymous]
    public async Task<IActionResult> ActiveEvents([FromRoute] bool? foralumni)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var events = foralumni.HasValue ? 
            await unitOfWork.EventsRepo.GetAll(x => x.IsValid == true &&
                x.AlumniEvent == foralumni.Value &&
                x.StartDate >= DateOnly.FromDateTime(DateTime.Today)):
             await unitOfWork.EventsRepo.GetAll(x => x.IsValid == true &&
                x.StartDate >= DateOnly.FromDateTime(DateTime.Today));

        if (events == null || !events.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<EventResponseDTO>>(events);

        var sorted = FinalData.OrderBy(x => x.StartDate);

        return Ok(ResponseModel<List<EventResponseDTO>>.Success(sorted.ToList(), "Events retrieved successfully."));
    }

    [HttpPost("get-alumni-events")]   
    public async Task<IActionResult> GetAlumniEvents([FromBody] SearchEventsRequestDTO requestDTO)
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
                : DateOnly.FromDateTime(new DateTime(int.Parse(requestDTO.ForSession.Split('-')[0]), 1, 1));

        if (!requestDTO.ToDate.HasValue)
            requestDTO.ToDate = string.IsNullOrEmpty(requestDTO.ForSession)
                ? DateOnly.FromDateTime(DateTime.Today)
                : DateOnly.FromDateTime(new DateTime(int.Parse(requestDTO.ForSession.Split('-')[1]), 3, 31));

        if (requestDTO.FromDate > requestDTO.ToDate)
            return Ok(ResponseModel<string>.Failure("Invalid Date Intervals!"));

        if (requestDTO.PageNumber <= 0 || requestDTO.PageSize <= 0)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        var eventsList = await unitOfWork.EventsRepo.GetAllEvents(requestDTO);

        if (eventsList == null || !eventsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        eventsList = eventsList.Where(x => x.AlumniEvent == true).ToList();
        if (eventsList == null || !eventsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        if (requestDTO.Category.HasValue && requestDTO.Category.Value != 0)
            eventsList = eventsList.Where(x => x.Category == requestDTO.Category.Value);

        if (eventsList == null || !eventsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        //var FinalData = mapper.Map<List<EventResponseDTO>>(eventsList);

        var sorted = eventsList.OrderBy(x => x.StartDate);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<EventResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = eventsList.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<EventResponseDTO>>.Success(response, "Details retrieved successfully."));
    }

    [HttpGet("event-by-id/{recordid}")]
    public async Task<IActionResult> GetEventById([FromRoute] long recordid)
    {  
        var eventDetails = await unitOfWork.EventsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (eventDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = mapper.Map<EventResponseDTO>(eventDetails);

        return Ok(ResponseModel<EventResponseDTO>.Success(returnData, "Details retrieved successfully."));
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] EventUpdateRequestDTO requestDTO, IFormFile? file = null)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!string.IsNullOrEmpty(requestDTO.VideoLink) && (!Uri.TryCreate(requestDTO.VideoLink, UriKind.Absolute, out var vUri) ||
            (vUri.Scheme != Uri.UriSchemeHttp && vUri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid Video URL!"));

        if (!string.IsNullOrEmpty(requestDTO.ExternalLink) && (!Uri.TryCreate(requestDTO.ExternalLink, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid External URL!"));

        if (requestDTO.StartDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Event cannot be scheduled in the past!"));

        if (requestDTO.EndDate < DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Invalid event end date!"));

        if (requestDTO.StartDate > requestDTO.EndDate)
            return Ok(ResponseModel<string>.Failure("Invalid Date Intervals!"));
        if (requestDTO.StartTime > requestDTO.EndTime)
            return Ok(ResponseModel<string>.Failure("Invalid Time Intervals!"));

        var eventDetails = await unitOfWork.EventsRepo.GetFirstOrDefault(r => r.RowId == requestDTO.RecordId);
        if (eventDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            if (!string.IsNullOrEmpty(eventDetails.BannerFileName))
                singleFileUpload.RemoveFile(eventDetails.BannerFilePath, eventDetails.BannerFileName);

            fileDetails = file != null
            ? await singleFileUpload.UploadFile(file, allowedExtensions, allowedMimeTypes, Constants.EVENTS, eventDetails.FinancialYear)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        eventDetails.Title = requestDTO.Title;
        eventDetails.Description = requestDTO.Description;
        eventDetails.Category = requestDTO.Category;
        eventDetails.StartDate = requestDTO.StartDate;
        eventDetails.EndDate = requestDTO.EndDate;
        eventDetails.AlumniEvent = requestDTO.AlumniEvent;
        eventDetails.StartTime = requestDTO.StartTime;
        eventDetails.EndTime = requestDTO.EndTime;
        eventDetails.Venue = requestDTO.Venue;
        eventDetails.OrganizedBy = requestDTO.OrganizedBy;       
        eventDetails.VideoLink = requestDTO.VideoLink;
        eventDetails.ExternalLink = requestDTO.ExternalLink;
     
        if (fileDetails != null)
        {
            eventDetails.BannerFileName = fileDetails?.FileName;
            eventDetails.BannerFileContentType = fileDetails?.FileMimeType;
            eventDetails.BannerFileExtn = fileDetails?.FileExtension;
            eventDetails.BannerFilePath = fileDetails?.FilePath;
        }

        eventDetails.ModifiedBy = BranchUserId;
        eventDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Event Details Updated Successfully.", StatusCodes.Status200OK));
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] long recordid, [FromRoute] bool status)
    {
        var eventDetails = await unitOfWork.EventsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (eventDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        eventDetails.IsValid = status;
        eventDetails.ModifiedBy = BranchUserId;
        eventDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Event Activated." : "Event Deactivated.", StatusCodes.Status200OK));
    }

    [HttpPost("mark-alumni-event/{recordid}/{isforalumni}")]
    public async Task<IActionResult> MarkAlumniEvent([FromRoute] long recordid, [FromRoute] bool isforalumni)
    {
        var eventDetails = await unitOfWork.EventsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (eventDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        eventDetails.AlumniEvent = isforalumni;
        eventDetails.ModifiedBy = BranchUserId;
        eventDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, isforalumni ? "Event marked for alumni." : "Event un-marked from alumni.", StatusCodes.Status200OK));               
    }
}
