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
public class DesignationController(IUnitOfWork UnitOfWork) : ControllerBase
{
    [HttpGet("all-designations")]
    public async Task<IActionResult> GetAllDesignations()
    {
        var returnData = await UnitOfWork.Designations.GetDesignations();

        if (returnData == null || returnData.Count > 0)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        return Ok(ResponseModel<List<DesignationDetailsDTO>?>.Success(returnData, "Designations retrieved successfully."));
    }

    [HttpGet("designation-by-id/{designationid}")]
    public async Task<IActionResult> GetDesignationById([FromRoute] int designationid)
    {
        var allDesignations = await UnitOfWork.Designations.GetDesignations();
        if (allDesignations == null || allDesignations.Count == 0)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        var returnData = allDesignations.FirstOrDefault(x => x.DesignationId == designationid);
        if (returnData == null)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        return Ok(ResponseModel<DesignationDetailsDTO?>.Success(returnData, "Designation details retrieved successfully."));
    }

    [HttpGet("designations-by-group/{designationgroup}")]
    public async Task<IActionResult> GetDesignationByGroup([FromRoute] int designationgroup)
    {
        var allDesignations = await UnitOfWork.Designations.GetDesignations();
        if (allDesignations == null || allDesignations.Count == 0)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        var returnData = allDesignations.Where(x => x.DesignationGroup == designationgroup);
        if (returnData == null || returnData.Count() > 0)
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        return Ok(ResponseModel<List<DesignationDetailsDTO>?>.Success(returnData.ToList(), "Designations retrieved successfully."));
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] NewDesignationDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var DataToBeSaved = new DesignationModel()
        {
            Title = requestDTO.Title,
            DesignationGroup = requestDTO.DesignationGroup,
            IsActive = true,
            IsGazetted = requestDTO.IsGazetted,
            CreatedBy = UserId,
            ModifiedBy = UserId
        };

        await UnitOfWork.Designations.Add(DataToBeSaved);
        await UnitOfWork.Save();

        return Ok(ResponseModel<int>.Success(DataToBeSaved.RowId, "Details submitted successfully.", StatusCodes.Status201Created));
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] DesignationUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        if (!await UnitOfWork.Designations.Exists(x => x.RowId == requestDTO.DesignationId))
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var returnval = await UnitOfWork.Designations.Update(requestDTO, UserId);
        if (!returnval)
            return Ok(ResponseModel<bool>.Failure("Failed to updated the details!"));
       
        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!"));
    }

    [HttpPost("update-status/{recordid}/{status}")]
    public async Task<IActionResult> UpdateStatus([FromRoute] int recordid, [FromRoute] bool status)
    {       
        if (!await UnitOfWork.Designations.Exists(x => x.RowId == recordid))
            return Ok(ResponseModel<string>.NoData("No Data Found!"));

        if(await UnitOfWork.EmployeeAppointmentRepo.Exists(x => x.Designation == recordid))
            return Ok(ResponseModel<string>.Failure("Designation already assigned to some employees. Kindly unassign the designation first."));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var returnval = await UnitOfWork.Designations.UpdateStatus(recordid, status, UserId);
        if (!returnval)
            return Ok(ResponseModel<bool>.Failure("Failed to updated the status!"));

        return Ok(ResponseModel<bool>.Success(true, status ? "Designation activated." : "Designation deactivated."));
    }
}
