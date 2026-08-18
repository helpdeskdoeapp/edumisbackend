using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Masters;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AcademicClassesController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AcademicClassesRequestDTO requestDTO)
    {
        if(requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!string.IsNullOrEmpty(requestDTO.ClassCode) && await unitOfWork.AcademicClassesRepo.Exists(x => x.ClassCode == requestDTO.ClassCode))
            return Ok(ResponseModel<string>.Failure("Class Code already exists!"));       

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var academicClassSaveObj = mapper.Map<AcademicClassesModel>(requestDTO);
        academicClassSaveObj.IsActive = true;
        academicClassSaveObj.CreatedBy = UserId;
        academicClassSaveObj.ModifiedBy = UserId;

        await unitOfWork.AcademicClassesRepo.Add(academicClassSaveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<int>.Success(academicClassSaveObj.RowId, "Details submitted successfully.", StatusCodes.Status201Created));       
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] AcademicClassesUpdateRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var academicClass = await unitOfWork.AcademicClassesRepo.GetFirstOrDefault(x => x.RowId == requestDTO.RecordId);
        if (academicClass == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        academicClass.Title = requestDTO.Title;
        academicClass.ClassCode = requestDTO.ClassCode;
        academicClass.Sections = requestDTO.Sections;
        academicClass.ModifiedBy = UserId;
        academicClass.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));
    }

    [HttpPost("updatestatus/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int recordid, [FromRoute] bool status)
    {
        var academicClass = await unitOfWork.AcademicClassesRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (academicClass == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        academicClass.IsActive = status;
        academicClass.ModifiedBy = UserId;
        academicClass.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Academic class activated." : "Academic class deactivated.", StatusCodes.Status200OK));       
    }

    [HttpGet("class-by-id/{recordid}")]
    public async Task<IActionResult> GetAcademicClassById([FromRoute] int recordid)
    {
        var academicClass = await unitOfWork.AcademicClassesRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (academicClass == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<AcademicClassesResponseDTO>(academicClass);

        return Ok(ResponseModel<AcademicClassesResponseDTO>.Success(responseDTO, "Details retrieved successfully"));     
    }

    [HttpGet("all-classes/{activeonly?}")]
    public async Task<IActionResult> GetAllAcademicClasses([FromRoute] bool? activeonly)
    {
        var academicClasses = await unitOfWork.AcademicClassesRepo.GetAll();
        if (academicClasses == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<List<AcademicClassesResponseDTO>>(academicClasses);

        var response = activeonly.HasValue && activeonly == true ?
            responseDTO.Where(x => x.IsActive).OrderBy(x => x.RecordId) :
            responseDTO.OrderBy(x => x.RecordId);

        return Ok(ResponseModel<List<AcademicClassesResponseDTO>>.Success(response.ToList(), "Details retrieved successfully"));
       
    }
}
