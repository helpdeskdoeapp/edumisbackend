using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models.Pagination;
using edumis.Models.Web;
using edumis.Models.Web.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Web;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MarqueeController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] MarqueeRequestDetailsDTO requestDTO)
    {
        if (requestDTO is null)
            return Ok(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var saveObj = mapper.Map<MarqueeDetailsModels>(requestDTO);
        saveObj.CreatedBy = UserId;
        saveObj.ModifiedBy = UserId;

        await unitOfWork.MarqueeDetailsRepo.Add(saveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<int>.Success(saveObj.RowId, "Details submitted successfully.", StatusCodes.Status201Created));
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] MarqueeDetailsUpdateRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return Ok(ResponseModel<string>.Failure("Invalid request"));

        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetFirstOrDefault(x => x.RowId == requestDTO.RecordId);
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        marqueeDetails.Title = requestDTO.Title;
        marqueeDetails.ShowNewIcon = requestDTO.ShowNewIcon;
        marqueeDetails.ExternalLink = requestDTO.ExternalLink;
        marqueeDetails.ModifiedBy = UserId;
        marqueeDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int recordid, [FromRoute] bool status)
    {
        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        marqueeDetails.IsValid = status;
        marqueeDetails.ModifiedBy = UserId;
        marqueeDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Marquee activated." : "Marquee deactivated.", StatusCodes.Status200OK));
    }

    [HttpPost("show-new-icon/{recordid}/{status}")]
    public async Task<IActionResult> ShowNewIcon([FromRoute] int recordid, [FromRoute] bool status)
    {
        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        marqueeDetails.ShowNewIcon = status;
        marqueeDetails.ModifiedBy = UserId;
        marqueeDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Marquee activated." : "Marquee deactivated.", StatusCodes.Status200OK));
    }

    [HttpGet("details-by-id/{recordid}")]
    public async Task<IActionResult> GetDetailsById([FromRoute] int recordid)
    {
        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<MarqueeDetailsResponseDTO>(marqueeDetails);

        return Ok(ResponseModel<MarqueeDetailsResponseDTO>.Success(responseDTO, "Details retrieved successfully"));
    }

    [HttpGet("list/{pagenumber}/{pagesize}")]
    public async Task<IActionResult> GetList([FromRoute] int pagenumber, [FromRoute] int pagesize)
    {
        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetAll();
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<List<MarqueeDetailsResponseDTO>>(marqueeDetails);


        var sorted = responseDTO.OrderByDescending(x => x.RecordId);

        var paginated = sorted
            .Skip((pagenumber - 1) * pagesize)
            .Take(pagesize)
            .ToList();

        var response = new PaginatedResponseDTO<MarqueeDetailsResponseDTO>
        {
            Items = paginated,
            PageNumber = pagenumber,
            PageSize = pagesize,
            TotalCount = responseDTO.Count()
        };

        return Ok(ResponseModel<PaginatedResponseDTO<MarqueeDetailsResponseDTO>>.Success(response, "Details retrieved successfully"));       
    }

    [HttpGet("current-active")]
    [AllowAnonymous]
    public async Task<IActionResult> CurrentActive()
    {
        var marqueeDetails = await unitOfWork.MarqueeDetailsRepo.GetAll(x=>x.IsValid == true);
        if (marqueeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<List<MarqueeDetailsResponseDTO>>(marqueeDetails);

        return Ok(ResponseModel<List<MarqueeDetailsResponseDTO>>.Success(responseDTO.OrderByDescending(x=>x.RecordId).ToList(), "Details retrieved successfully"));
    }
}
