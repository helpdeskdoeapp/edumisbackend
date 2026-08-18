using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models.Library.Newspaper;
using edumis.Models.Library.Newspaper.DTO;
using edumis.Models.Pagination;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Library.NewsPapers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NewspaperController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] NewspaperRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var saveObj = mapper.Map<NewspaperRequestDTO, NewspaperModel>(requestDTO);
        saveObj.NewspaperId = Guid.NewGuid();
        saveObj.BranchId = BranchID;
        saveObj.CreatedBy = BranchUserId;
        saveObj.ModifiedBy = BranchUserId;      
        await unitOfWork.NewspaperRepo.Add(saveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<Guid>.Success(saveObj.NewspaperId, "Details Saved Successfully!", StatusCodes.Status201Created));       
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] NewspaperUpdateRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var newsPaperDetails = await unitOfWork.NewspaperRepo.GetFirstOrDefault(x => x.NewspaperId == new Guid(requestDTO.NewspaperId));
        if(newsPaperDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        newsPaperDetails.Frequency = requestDTO.Frequency;
        newsPaperDetails.Price = requestDTO.Price;
        newsPaperDetails.Quantity = requestDTO.Quantity;
        newsPaperDetails.Language = requestDTO.Language;
        newsPaperDetails.Description = requestDTO.Description;
        newsPaperDetails.EBookUrl = requestDTO.EBookUrl;
        newsPaperDetails.Genre = requestDTO.Genre;
        newsPaperDetails.Title = requestDTO.Title;
        newsPaperDetails.ModifiedBy = BranchUserId;    
        newsPaperDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] string recordid, [FromRoute] bool status)
    {        
        var newsPaperDetails = await unitOfWork.NewspaperRepo.GetFirstOrDefault(x => x.NewspaperId == new Guid(recordid));
        if (newsPaperDetails == null)
            return Ok(ResponseModel<string>.NoData("No details found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;
               
        newsPaperDetails.IsActive = status;
        newsPaperDetails.ModifiedBy = BranchUserId;
        newsPaperDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Activated Successfully." : "De-activated Successfully.", StatusCodes.Status200OK));       
    }


    [HttpGet("details/{recordid}")]
    public async Task<IActionResult> GetDetails([FromRoute] string recordid)
    {
        if (string.IsNullOrEmpty(recordid))
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var returnDetails = await unitOfWork.NewspaperRepo.GetDetails(new Guid(recordid));
        if (returnDetails == null)
            return Ok(ResponseModel<string>.NoData("No details found!"));

        return Ok(ResponseModel<NewspaperDetailsResponseDTO>.Success(returnDetails, "Details retrieved successfully"));   
    }

    [HttpGet("search/{pagenumber}/{pagesize}")]
    public async Task<IActionResult> Search([FromRoute] int pagenumber, [FromRoute] int pagesize)
    {
        string BranchID = User.FindFirst("Branch")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(BranchID))
            return BadRequest(ResponseModel<string>.Failure("Unauthorised User!", StatusCodes.Status401Unauthorized));

        var allData = await unitOfWork.NewspaperRepo.GetNewspapers(BranchID);
        if (allData == null || !allData.Any())
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var sorted = allData.OrderBy(x => x.Title);

        var paginated = sorted
            .Skip((pagenumber - 1) * pagesize)
            .Take(pagesize)
            .ToList();

        var response = new PaginatedResponseDTO<NewspaperDetailsResponseDTO>
        {
            Items = paginated,
            PageNumber = pagenumber,
            PageSize = pagesize,
            TotalCount = allData.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<NewspaperDetailsResponseDTO>>.Success(response, "Details retrieved successfully"));       
    }
}
