using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.News;
using edumis.Models.News.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.News;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NewsController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{
    #region News API Methods
    [HttpPost("add")]    
    public async Task<IActionResult> Add([FromForm] NewsRequestDTO NewsDetails, IFormFile? file = null)
    {
        if (!string.IsNullOrEmpty(NewsDetails.VideoLink) && (!Uri.TryCreate(NewsDetails.VideoLink, UriKind.Absolute, out var vUri) ||
           (vUri.Scheme != Uri.UriSchemeHttp && vUri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid Video URL!"));

        if (!string.IsNullOrEmpty(NewsDetails.ExternalLink) && (!Uri.TryCreate(NewsDetails.ExternalLink, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid External URL!"));

        if(NewsDetails.NewsDate > DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Invalid News Date!"));

        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.NoData("Unable to fetch current financial year!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            fileDetails = file != null
            ? await singleFileUpload.UploadFileForSession(file, allowedExtensions, allowedMimeTypes, Constants.NEWS, currentSessionData.ForSession, NewsDetails.NewsDate.ToString("dd-MM-yyyy"))
            : null;
           
            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var saveObj = new NewsModel
        {
            FinancialYear = currentSessionData.ForSession,
            Title = NewsDetails.Title,
            Description = NewsDetails.Description,           
            ExternalLink = NewsDetails.ExternalLink,
            VideoLink = NewsDetails.VideoLink,
            NewsDate = NewsDetails.NewsDate,
            BannerFileName = fileDetails?.FileName,
            BannerFilePath = fileDetails?.FilePath,
            BannerFileExtn = fileDetails?.FileExtension,
            BannerFileContentType = fileDetails?.FileMimeType,
            IsValid = true,
            AlumniNews = NewsDetails.AlumniNews,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };

        await unitOfWork.NewsRepo.Add(saveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success(saveObj.RowId.ToString(), "News Details Saved!", StatusCodes.Status201Created));       
    }

    [HttpPost("search/{activeonly?}")]    
    public async Task<IActionResult> SearchNews([FromBody] SearchNewsRequestDTO requestDTO, [FromRoute] bool? activeonly)
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

        var newsList = requestDTO.AlumniNews.HasValue && requestDTO.AlumniNews.Value == true ?
                await unitOfWork.NewsRepo.GetAll(x =>
                    x.FinancialYear == requestDTO.ForSession &&
                    x.NewsDate >= requestDTO.FromDate &&
                    x.NewsDate <= requestDTO.ToDate &&
                    x.AlumniNews == true)
            :
                await unitOfWork.NewsRepo.GetAll(x =>
                    x.FinancialYear == requestDTO.ForSession &&
                    x.NewsDate >= requestDTO.FromDate &&
                    x.NewsDate <= requestDTO.ToDate &&
                    x.AlumniNews != true);

        if (newsList == null || !newsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<NewsDetailResponseDTO>>(newsList);

        if (activeonly.HasValue)
            FinalData = FinalData.Where(x => x.IsValid == activeonly.Value).ToList();

        var sorted = FinalData.OrderByDescending(x => x.NewsDate);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<NewsDetailResponseDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = FinalData.Count
        };

        return Ok(ResponseModel<PaginatedResponseDTO<NewsDetailResponseDTO>>.Success(response, "Details retrieved successfully."));      
    }

    [HttpGet("get-active-news/{pagenumber}/{pagesize}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentNews([FromRoute] int pagenumber, [FromRoute] int pagesize)
    {
        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        if (pagenumber <= 0 || pagesize <= 0)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        var newsList = await unitOfWork.NewsRepo.GetAll(x =>
                x.FinancialYear == currentSessionData.ForSession &&
                x.IsValid == true &&
                x.AlumniNews != true
            );

        if (newsList == null || !newsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<NewsDetailResponseDTO>>(newsList);

        var sorted = FinalData.OrderByDescending(x => x.NewsDate);

        var paginated = sorted
            .Skip((pagenumber - 1) * pagesize)
            .Take(pagesize)
            .ToList();

        var response = new PaginatedResponseDTO<NewsDetailResponseDTO>
        {
            Items = paginated,
            PageNumber = pagenumber,
            PageSize = pagesize,
            TotalCount = FinalData.Count
        };

        return Ok(ResponseModel<PaginatedResponseDTO<NewsDetailResponseDTO>>.Success(response, "Details retrieved successfully."));
    }

    [HttpGet("get-current-alumni-news/{pagenumber}/{pagesize}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrentAlumniNews([FromRoute] int pagenumber, [FromRoute] int pagesize)
    {
        var currentSessionData = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true);
        if (currentSessionData == null)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        if (pagenumber <= 0 || pagesize <= 0)
            return Ok(ResponseModel<string>.Failure("Invalid Pagination Parameters!"));

        var newsList = await unitOfWork.NewsRepo.GetAll(x =>
                x.FinancialYear == currentSessionData.ForSession &&
                x.IsValid == true &&
                x.AlumniNews == true
            );

        if (newsList == null || !newsList.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var FinalData = mapper.Map<List<NewsDetailResponseDTO>>(newsList);

        var sorted = FinalData.OrderByDescending(x => x.NewsDate);

        var paginated = sorted
            .Skip((pagenumber - 1) * pagesize)
            .Take(pagesize)
            .ToList();

        var response = new PaginatedResponseDTO<NewsDetailResponseDTO>
        {
            Items = paginated,
            PageNumber = pagenumber,
            PageSize = pagesize,
            TotalCount = FinalData.Count
        };

        return Ok(ResponseModel<PaginatedResponseDTO<NewsDetailResponseDTO>>.Success(response, "Details retrieved successfully."));
    }

    [HttpGet("news-by-id/{recordid}")]
    public async Task<IActionResult> GetNewsById([FromRoute] int recordid)
    {  
        var newsDetails = await unitOfWork.NewsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (newsDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = mapper.Map<NewsDetailResponseDTO>(newsDetails);

        return Ok(ResponseModel<NewsDetailResponseDTO>.Success(returnData, "Details retrieved successfully."));
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] NewsUpdateRequestDTO requestDTO, IFormFile? file = null)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!string.IsNullOrEmpty(requestDTO.VideoLink) && (!Uri.TryCreate(requestDTO.VideoLink, UriKind.Absolute, out var vUri) ||
          (vUri.Scheme != Uri.UriSchemeHttp && vUri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid Video URL!"));

        if (!string.IsNullOrEmpty(requestDTO.ExternalLink) && (!Uri.TryCreate(requestDTO.ExternalLink, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)))
            return Ok(ResponseModel<string>.Failure("Invalid External URL!"));

        if (requestDTO.NewsDate > DateOnly.FromDateTime(DateTime.Today))
            return Ok(ResponseModel<string>.Failure("Invalid News Date!"));

        var newsDetails = await unitOfWork.NewsRepo.GetFirstOrDefault(x => x.RowId == requestDTO.RecordId);
        if (newsDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            if (!string.IsNullOrEmpty(newsDetails.BannerFileName))
                singleFileUpload.RemoveFile(newsDetails.BannerFilePath, newsDetails.BannerFileName);

            fileDetails = file != null
            ? await singleFileUpload.UploadFileForSession(file, allowedExtensions, allowedMimeTypes, Constants.NEWS, newsDetails.FinancialYear, requestDTO.NewsDate.ToString("dd-MM-yyyy"))
            : null;
            
            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        newsDetails.Title = requestDTO.Title;
        newsDetails.Description = requestDTO.Description;
        newsDetails.NewsDate = requestDTO.NewsDate;
        newsDetails.VideoLink = requestDTO.VideoLink;
        newsDetails.ExternalLink = requestDTO.ExternalLink;
        newsDetails.AlumniNews = requestDTO.AlumniNews;

        if (fileDetails != null)
        {
            newsDetails.BannerFileName = fileDetails?.FileName;
            newsDetails.BannerFileContentType = fileDetails?.FileMimeType;
            newsDetails.BannerFileExtn = fileDetails?.FileExtension;
            newsDetails.BannerFilePath = fileDetails?.FilePath;
        }

        newsDetails.ModifiedBy = BranchUserId;
        newsDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "News Details Updated Successfully.", StatusCodes.Status200OK));
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] long recordid, [FromRoute] bool status)
    {
        var newsDetails = await unitOfWork.NewsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (newsDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        newsDetails.IsValid = status;
        newsDetails.ModifiedBy = BranchUserId;
        newsDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "News Activated." : "News Deactivated.", StatusCodes.Status200OK));       
    }

    [HttpPost("mark-alumni-news/{recordid}/{isforalumni}")]
    public async Task<IActionResult> MarkAlumniNews([FromRoute] long recordid, [FromRoute] bool isforalumni)
    {
        var newsDetails = await unitOfWork.NewsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (newsDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        newsDetails.AlumniNews = isforalumni;
        newsDetails.ModifiedBy = BranchUserId;
        newsDetails.ModifiedDate = DateTime.Now;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, isforalumni ? "News marked for alumni." : "News un-marked from alumni.", StatusCodes.Status200OK));
    }
    #endregion
}
