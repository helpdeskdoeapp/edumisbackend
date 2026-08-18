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
public class AcademicSubjectsController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AcademicSubjectsRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!string.IsNullOrEmpty(requestDTO.SubjectCode) && await unitOfWork.AcademicSubjectsRepo.Exists(x => x.SubjectCode == requestDTO.SubjectCode))
            return Ok(ResponseModel<string>.Failure("Subject Code already exists!"));
    
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var subjectSaveObj = mapper.Map<AcademicSubjectsModel>(requestDTO);
        subjectSaveObj.IsActive = true;
        subjectSaveObj.CreatedBy = UserId;
        subjectSaveObj.ModifiedBy = UserId;

        await unitOfWork.AcademicSubjectsRepo.Add(subjectSaveObj);
        await unitOfWork.Save();

        return Ok(ResponseModel<int>.Success(subjectSaveObj.RowId, "Details submitted successfully.", StatusCodes.Status201Created));        
    }   

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] AcademicSubjectsUpdateRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var academicSubject = await unitOfWork.AcademicSubjectsRepo.GetFirstOrDefault(x => x.RowId == requestDTO.RecordId);
        if (academicSubject == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        academicSubject.Title = requestDTO.Title;
        academicSubject.SubjectCode = requestDTO.SubjectCode;      
        academicSubject.ModifiedBy = UserId;
        academicSubject.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));        
    }

    [HttpPost("updatestatus/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int recordid, [FromRoute] bool status)
    {
        var academicSubject = await unitOfWork.AcademicClassesRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (academicSubject == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        academicSubject.IsActive = status;
        academicSubject.ModifiedBy = UserId;
        academicSubject.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Academic subject activated." : "Academic subject deactivated.", StatusCodes.Status200OK));       
    }

    [HttpGet("subject-by-id/{recordid}")]
    public async Task<IActionResult> GetAcademicSubjectById([FromRoute] int recordid)
    {
        var academicSubject = await unitOfWork.AcademicSubjectsRepo.GetFirstOrDefault(x => x.RowId == recordid);
        if (academicSubject == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var responseDTO = mapper.Map<AcademicSubjectsResponseDTO>(academicSubject);

        return Ok(ResponseModel<AcademicSubjectsResponseDTO>.Success(responseDTO, "Details retrieved successfully"));
    }

    [HttpGet("all-subjects/{activeonly?}")]
    public async Task<IActionResult> GetAllAcademicSubjects([FromRoute] bool? activeonly)
    {
        var academicSubjects = await unitOfWork.AcademicSubjectsRepo.GetAll();
        if (academicSubjects == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));


        var responseDTO = mapper.Map<List<AcademicSubjectsResponseDTO>>(academicSubjects);


        var response = activeonly.HasValue && activeonly == true ?
           responseDTO.Where(x => x.IsActive).OrderBy(x => x.RecordId) :
           responseDTO.OrderBy(x => x.RecordId);

        return Ok(ResponseModel<List<AcademicSubjectsResponseDTO>>.Success(response.ToList(), "Details retrieved successfully"));    
    }
}
