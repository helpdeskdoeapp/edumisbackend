using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.AppConstants.Library;
using edumis.Models.Library.Magazine;
using edumis.Models.Library.Magazine.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.Magazines;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MagazineController(IUnitOfWork unitOfWork, IMapper mapper, SingleFileUpload singleFileUpload) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromForm] MagazineRequestDTO requestDTO, IFormFile? file = null)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        UploadedFileDetailsModel? fileDetails = null;
        if (file != null)
        {
            string[] allowedExtensions = Constants.AllowedExtensions;
            string[] allowedMimeTypes = Constants.AllowedMimeTypes;

            fileDetails = file != null
            ? await singleFileUpload.UploadFileInSubFolder(file, allowedExtensions, allowedMimeTypes, Constants.Library, BranchID, LibraryContentFolders.MegazineSubfolder)
            : null;

            if (fileDetails == null)
                return Ok(ResponseModel<string>.Failure("File Upload Failed!"));
        }

        var saveObj = mapper.Map<MagazineRequestDTO, MagazineModel>(requestDTO);
        saveObj.MagazineId = Guid.NewGuid();
        saveObj.BranchId = BranchID;
        saveObj.TotalQty = requestDTO.ProcurementDetails.Quantity;
        saveObj.CreatedBy = BranchUserId;
        saveObj.ModifiedBy = BranchUserId;
        if (fileDetails != null)
        {
            saveObj.CoverImageUrl = fileDetails.FilePath;
            saveObj.CoverImageExtenstion = fileDetails.FileExtension;
            saveObj.CoverImageContentType = fileDetails.FileMimeType;
        }
        await unitOfWork.MagazineRepo.Add(saveObj);

        var TransactionSaveObj = mapper.Map<MagazineProcurementTransactionRequestDTO, MagazineProcurementTransactionModel>(requestDTO.ProcurementDetails);
        TransactionSaveObj.MagazineId = saveObj.MagazineId;
        TransactionSaveObj.TransactionId = Guid.NewGuid();
        TransactionSaveObj.CreatedBy = BranchUserId;
        TransactionSaveObj.ModifiedBy = BranchUserId;
        await unitOfWork.MagazineProcurementTransactionRepo.Add(TransactionSaveObj);
        
        await unitOfWork.Save();

        return Ok(ResponseModel<Guid>.Success(saveObj.MagazineId, "Details Saved Successfully!", StatusCodes.Status201Created));       
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] MagazineUpdateRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var magazineDetails = await unitOfWork.MagazineRepo.GetFirstOrDefault(x => x.MagazineId == new Guid(requestDTO.MagazineId));
        if (magazineDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));
      
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        magazineDetails.Title = requestDTO.Title;
        magazineDetails.Publisher = requestDTO.Publisher;
        magazineDetails.Editor = requestDTO.Editor;
        magazineDetails.Edition = requestDTO.Edition;
        magazineDetails.Language = requestDTO.Language;
        magazineDetails.Frequency = requestDTO.Frequency;
        magazineDetails.Genre = requestDTO.Genre;
        magazineDetails.Description = requestDTO.Description;
        magazineDetails.Notes = requestDTO.Notes;
        magazineDetails.Tags = requestDTO.Tags;
        magazineDetails.EBookUrl = requestDTO.EBookUrl;
        magazineDetails.AudioUrl = requestDTO.AudioUrl;
        magazineDetails.VideoUrl = requestDTO.VideoUrl;        
        magazineDetails.ModifiedBy = BranchUserId;
        magazineDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("updatecoverimage/{magazineid}")]
    public async Task<IActionResult> UpdateCoverImage([FromRoute] string magazineid, IFormFile file)
    {
        if (string.IsNullOrEmpty(magazineid) || file == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var magazineDetails = await unitOfWork.MagazineRepo.GetFirstOrDefault(x => x.MagazineId == new Guid(magazineid));

        if (magazineDetails == null)
            return Ok(ResponseModel<string>.NoData("No magazine details found!"));

        string[] allowedExtensions = Constants.AllowedImageExtensions;
        string[] allowedMimeTypes = Constants.AllowedImageMimeTypes;

        var fileDetails = await singleFileUpload.UploadFileInFolder(file, allowedExtensions, allowedMimeTypes, Constants.Library, BranchID);
        if (fileDetails == null)
            return Ok(ResponseModel<string>.Failure("File Upload Failed!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        if (!string.IsNullOrEmpty(magazineDetails.CoverImageUrl))
            singleFileUpload.RemoveFile(magazineDetails.CoverImageUrl);

        magazineDetails.CoverImageUrl = fileDetails.FilePath;
        magazineDetails.CoverImageExtenstion = fileDetails.FileExtension;
        magazineDetails.CoverImageContentType = fileDetails.FileMimeType;
        magazineDetails.ModifiedBy = BranchUserId;
        magazineDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Magazine Cover Image Updated Successfully!", StatusCodes.Status200OK));
    }

    [HttpGet("details/{recordid}")]
    public async Task<IActionResult> GetDetails([FromRoute] string recordid)
    {
        if (string.IsNullOrEmpty(recordid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var returnDetails = await unitOfWork.MagazineRepo.GetDetails(new Guid(recordid));
        if (returnDetails == null)
            return NotFound(ResponseModel<string>.NoData("No details found!"));

        return Ok(ResponseModel<MagazineDetailsReponseDTO>.Success(returnDetails, "Book details retrieved successfully"));     
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] SearchMagazineRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var allMagazines = await unitOfWork.MagazineRepo.GetMagazines(BranchID);
        if (allMagazines == null || !allMagazines.Any())
            return Ok(ResponseModel<string>.NoData("No record found!"));

        if (requestDTO.Language.HasValue && requestDTO.Language > 0)
            allMagazines = allMagazines.Where(x => x.Language == requestDTO.Language);

        if (!string.IsNullOrEmpty(requestDTO.Title))
            allMagazines = allMagazines.Where(x => x.Title.Contains(requestDTO.Title, StringComparison.OrdinalIgnoreCase));
              
        if (!string.IsNullOrEmpty(requestDTO.Publisher))
            allMagazines = allMagazines.Where(x => x.Publisher != null && x.Publisher.Contains(requestDTO.Publisher, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(requestDTO.Editor))
            allMagazines = allMagazines.Where(x => x.Editor != null && x.Editor.Contains(requestDTO.Editor, StringComparison.OrdinalIgnoreCase));

        if (requestDTO.Rating.HasValue && requestDTO.Rating > 0)
            allMagazines = allMagazines.Where(x => x.Rating.HasValue && x.Rating >= requestDTO.Rating);

        if (requestDTO.Tags != null && requestDTO.Tags.Any())
            allMagazines = allMagazines.Where(x => x.Tags != null && requestDTO.Tags.Any(tag => x.Tags.Contains(tag, StringComparison.OrdinalIgnoreCase)));

        if (!allMagazines.Any())
            return Ok(ResponseModel<string>.NoData("No record found!"));

        var searchResults = allMagazines.Select(mag => new MagazineSearchResultDTO
        {
            MagazineId = mag.MagazineId,
            BranchId = mag.BranchId,
            BranchName = mag.BranchName,
            Title = mag.Title,
            Publisher = mag.Publisher,
            Editor = mag.Editor,
            Edition = mag.Edition,
            Language = mag.Language,
            LanguageDesc = mag.LanguageDesc,
            Frequency = mag.Frequency,
            FrequencyDesc = mag.FrequencyDesc,
            Description = mag.Description,
            CoverImageUrl = mag.CoverImageUrl,
            CoverImageExtenstion = mag.CoverImageExtenstion,
            CoverImageContentType = mag.CoverImageContentType,
            Notes = mag.Notes,
            Tags = mag.Tags,
            Rating = mag.Rating,
            Qty = mag.TotalQty,
            AvailableQty = mag.AvailableQty,
            EBookUrl = mag.EBookUrl,
            AudioUrl = mag.AudioUrl,
            VideoUrl = mag.VideoUrl
        }).ToList();

        var sorted = searchResults.OrderBy(x => x.Title);

        var paginated = sorted
            .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
            .Take(requestDTO.PageSize)
            .ToList();

        var response = new PaginatedResponseDTO<MagazineSearchResultDTO>
        {
            Items = paginated,
            PageNumber = requestDTO.PageNumber,
            PageSize = requestDTO.PageSize,
            TotalCount = searchResults.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<MagazineSearchResultDTO>>.Success(response, "Details retrieved successfully"));
    }

}
