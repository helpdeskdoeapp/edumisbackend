using AutoMapper;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Global;
using edumis.Models.Global.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GlobalController(IUnitOfWork UnitOfWork, IMapper Mapper) : ControllerBase
{    
    #region Master Codes API Methods       
    [HttpGet("all-master-codes")]    
    public async Task<IActionResult> GetMasterCodes()
    {
        var mastercodes = await UnitOfWork.Codes.GetMasterCodeDetails();
        if (mastercodes == null || mastercodes.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = Mapper.Map<List<MasterCodeDetailsResponseDTO>>(mastercodes);
        return Ok(ResponseModel<List<MasterCodeDetailsResponseDTO>>.Success(returnData.OrderBy(x => x.Code).ToList(), "Details retrieved successfully"));
    }

    [HttpPost("master-codes")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMasterCodes([FromBody] List<int> code)
    {
        var codeDataList = await UnitOfWork.Codes.GetMasterCodeDetails(code);

        if (codeDataList == null || codeDataList.Count() == 0)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = Mapper.Map<List<MasterCodeDetailsResponseDTO>>(codeDataList);
        return Ok(ResponseModel<List<MasterCodeDetailsResponseDTO>>.Success(returnData.OrderBy(x => x.Code).ToList(), "Details retrieved successfully"));
    }


    [HttpGet("master-code-details/{id}")]    
    public async Task<IActionResult> GetMasterCodeById([FromRoute] int id)
    {
        var masterCodeDetails = await UnitOfWork.Codes.GetMasterCodeDetails(id);
        if (masterCodeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var ReturnData = Mapper.Map<MasterCodeDetailsResponseDTO>(masterCodeDetails);

        return Ok(ResponseModel<MasterCodeDetailsResponseDTO>.Success(ReturnData, "Details retrieved successfully"));
    }

    [HttpPost("add-master-code")]  
    public async Task<ActionResult> AddMasterCode([FromBody] MasterCodeRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var allCodes = await UnitOfWork.Codes.GetAll();
       
        var SaveObj = new CodesModel()
        {
            Code = (allCodes?.Select(x => x.Code).DefaultIfEmpty(0).Max() ?? 0) + 1,
            CodeDescription = requestDTO.CodeDescription,
            IsActive = true,
            CreatedBy = UserId,
            ModifiedBy = UserId
        };

        await UnitOfWork.Codes.Add(SaveObj);
        await UnitOfWork.Save();
        return Ok(ResponseModel<int>.Success(SaveObj.Code, "Details submitted successfully.", StatusCodes.Status201Created));
    }

    [HttpPost("update-master-code")]   
    public async Task<IActionResult> UpdateMasterCode([FromBody] MasterCodeUpdateRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var codeDetails = await UnitOfWork.Codes.GetFirstOrDefault(x => x.Code == requestDTO.Code);

        if (codeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        codeDetails.CodeDescription = requestDTO.CodeDescription;
        codeDetails.ModifiedBy = UserId;
        codeDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));       
    }

    [HttpPost("update-master-code-status/{code}/{status}")]   
    public async Task<IActionResult> UpdateMasterCodeStatus([FromRoute] int code, [FromRoute] bool status)
    {       
        var codeDetails = await UnitOfWork.Codes.GetFirstOrDefault(x => x.Code == code);

        if (codeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        codeDetails.IsActive = status;
        codeDetails.ModifiedBy = UserId;
        codeDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Master code activated." : "Master code deactivated.", StatusCodes.Status200OK));
    }
    #endregion

    #region For Master Codes' Sub-codes API
    [HttpGet("subcode-details/{subcode}")]   
    public async Task<IActionResult> GetCodeValueDescription([FromRoute] int subcode)
    {
        var codeValueDetails = await UnitOfWork.CodeValues.GetFirstOrDefault(x => x.CodeValue == subcode);

        if (codeValueDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnData = Mapper.Map<MasterCodeValueDetailsDTO>(codeValueDetails);
        return Ok(ResponseModel<MasterCodeValueDetailsDTO>.Success(returnData, "Details retrieved successfully"));
    }

    [HttpPost("add-subcode")]    
    public async Task<IActionResult> AddMasterSubCode([FromBody] MasterSubCodeRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var subCodes = await UnitOfWork.CodeValues.GetAll(x => x.Code == requestDTO.Code);

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        CodeValuesModel DataToBeSaved = new CodeValuesModel()
        {
            Code = requestDTO.Code,
            CodeValue = (subCodes?.Select(x=>x.CodeValue).DefaultIfEmpty(requestDTO.Code * 100).Max() ?? requestDTO.Code * 100) + 1,
            CodeValDescription = requestDTO.SubCodeDescription,
            ParentCode = requestDTO.ParentCode,
            IsActive = true,
            CreatedBy = UserId,           
            ModifiedBy = UserId
        };

        await UnitOfWork.CodeValues.Add(DataToBeSaved);
        await UnitOfWork.Save();

        return Ok(ResponseModel<int>.Success(DataToBeSaved.CodeValue, "Details submitted successfully.", StatusCodes.Status201Created));        
    }

    [HttpPost("update-subcode")]    
    public async Task<IActionResult> UpdateSubCode([FromBody] MasterSubCodeUpdateRequestDTO requestDTO)
    {
        if (requestDTO is null)
            return BadRequest(ResponseModel<string>.Failure("Invalid request"));

        var subCodeDetails = await UnitOfWork.CodeValues.GetFirstOrDefault(x => x.CodeValue == requestDTO.SubCode);
        if (subCodeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        subCodeDetails.CodeValDescription = requestDTO.SubCodeDescription;
        subCodeDetails.ParentCode = requestDTO.ParentCode;
        subCodeDetails.ModifiedBy = UserId;
        subCodeDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Details Updated Successfully!", StatusCodes.Status200OK));     
    }

    [HttpPost("update-subcode-status/{subcode}/{status}")]   
    public async Task<IActionResult> UpdateMasterSubCodeStatus([FromRoute] int subcode, [FromRoute] bool status)
    {
        var codeDetails = await UnitOfWork.CodeValues.GetFirstOrDefault(x => x.CodeValue == subcode);

        if (codeDetails == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        codeDetails.IsActive = status;
        codeDetails.ModifiedBy = UserId;
        codeDetails.ModifiedDate = DateTime.UtcNow;

        await UnitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, status ? "Master sub-code activated." : "Master sub-code deactivated.", StatusCodes.Status200OK));
    }
    #endregion

    #region Academic Session APIs
    [HttpPost("updatesessioninfo")]
    public async Task<IActionResult> UpdateAcademicSession([FromBody] SessionInfoDTO SessionDetails)
    {
        if (string.IsNullOrEmpty(SessionDetails.ForSession))
            return Ok(ResponseModel<string>.Failure("Invalid Session!"));

        if (!await UnitOfWork.AcademicSessions.Exists(x => x.ForSession == SessionDetails.ForSession))
            return Ok(ResponseModel<string>.NoData("No data found!"));

        bool returnval = await UnitOfWork.AcademicSessions.Update(SessionDetails);
        if (!returnval)
            return Ok(ResponseModel<string>.Failure("Failed To Update Academic Session Details!"));

        return Ok(ResponseModel<bool>.Success(true, "Academic Session Details Updated Successfully.", StatusCodes.Status200OK));       
    }

    [HttpGet("sessiondetail/{session}")]
    public async Task<ActionResult<SessionDetailsDTO>> GetAcademicSessionDetails([FromRoute] string session)
    {
        var ReturnData = await UnitOfWork.AcademicSessions.GetFirstOrDefault(x => x.ForSession == session);
        if (ReturnData == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var returnDetails = new SessionDetailsDTO(
                    ReturnData.ForSession,
                    ReturnData.IsValid,
                    ReturnData.IsCurrent,
                    ReturnData.RegistrationStartDate,
                    ReturnData.RegistrationEndDate,
                    ReturnData.LateRegistrationStartDate,
                    ReturnData.LateRegistrationEndDate,
                    ReturnData.RegistrationEndTime,
                    ReturnData.LateRegistrationEndTime,
                    ReturnData.Reg_AgeAsOnDate,
                    ReturnData.RegistrationStartTime,
                    ReturnData.LateRegistrationStartTime,
                    ReturnData.IsRegistrationOpen
                );

        return Ok(ResponseModel<SessionDetailsDTO>.Success(returnDetails, "Details retrieved successfully"));
    }

    [HttpGet("allsessions")]    
    public async Task<ActionResult<List<SessionDetailsDTO>>> GetAllAcademicSessions()
    {
        var ReturnData = await UnitOfWork.AcademicSessions.GetAll();
        if (ReturnData == null)
            return Ok(ResponseModel<string>.NoData("No data found!"));

        var allSessions = ReturnData.OrderBy(x => x.ForSession).Select(x => new SessionDetailsDTO(
                    x.ForSession,
                    x.IsValid,
                    x.IsCurrent,
                    x.RegistrationStartDate,
                    x.RegistrationEndDate,
                    x.LateRegistrationStartDate,
                    x.LateRegistrationEndDate,
                    x.RegistrationEndTime,
                    x.LateRegistrationEndTime,
                    x.Reg_AgeAsOnDate,
                    x.RegistrationStartTime,
                    x.LateRegistrationStartTime,
                    x.IsRegistrationOpen
            )).ToList();        
        return Ok(ResponseModel<List<SessionDetailsDTO>>.Success(allSessions, "Details retrieved successfully"));        
    }
    #endregion
    
    #region Designation User Type Mapping
    [HttpPost("mapdesig_with_usertype")]  
    public async Task<ActionResult<bool>> AddDesignationUserTypeMapping([FromBody] DesignationUserTypeMappingDTO MappingData)
    {
        if (MappingData == null) return BadRequest(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Invalid Data Input!.",
            Success = false,
            ReturnCode = StatusCodes.Status400BadRequest.ToString()
        });
        if (await UnitOfWork.DesignationUserTypeMapping.Exists(x => x.DesignationId == MappingData.DesignationId && x.UserType == MappingData.UserType))
            return BadRequest(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "Mapping Already Exists.",
                Success = false,
                ReturnCode = StatusCodes.Status400BadRequest.ToString()
            });

        DesignationUserTypeMapping mappingObj = new DesignationUserTypeMapping()
        {
            DesignationId = MappingData.DesignationId,
            UserType = MappingData.UserType,
            CreatedBy = MappingData.UserId,
            ModifiedBy = MappingData.UserId
        };
        await UnitOfWork.DesignationUserTypeMapping.Add(mappingObj);
        await UnitOfWork.Save();

        return Ok(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Mapping Done Successfully.",
            Success = true,
            ReturnCode = StatusCodes.Status201Created.ToString()
        });
    }

    [HttpGet("alldesig_usertypemappings")]   
    public async Task<ActionResult<List<DesignationUserTypeMappingDetailsDTO>>> GetAllDesignationUserTypeMappings()
    {
        var allmappings = await UnitOfWork.DesignationUserTypeMapping.GetAllMappings();
        if (allmappings == null)
            return NotFound(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "No Data Found.",
                Success = false,
                ReturnCode = StatusCodes.Status404NotFound.ToString()
            });
        return Ok(allmappings);
    }

    [HttpPost("deletedesig_usertypemapping/{designationid}/{usertype}")]    
    public async Task<ActionResult> DeleteDesignationUserTypeMappings([FromRoute] int designationid, [FromRoute] int usertype)
    {
        var selectedmapping = await UnitOfWork.DesignationUserTypeMapping.GetFirstOrDefault(x => x.DesignationId == designationid && x.UserType == usertype);
        if (selectedmapping == null)
            return NotFound(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "No Data Found.",
                Success = false,
                ReturnCode = StatusCodes.Status404NotFound.ToString()
            });

        await UnitOfWork.DesignationUserTypeMapping.Remove(selectedmapping);

        return Ok(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Mapping Removed Successfully.",
            Success = true,
            ReturnCode = StatusCodes.Status200OK.ToString()
        });
    }
    #endregion

    #region Designation Menu Mapping
    [HttpPost("mapdesignationwithmenu")]    
    public async Task<ActionResult<bool>> MapDesignationWithMenu([FromBody] DesignationMenuItemsDTO MappingData)
    {
        if (MappingData == null) return BadRequest(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Invalid Data Input!.",
            Success = false,
            ReturnCode = StatusCodes.Status400BadRequest.ToString()
        });

        if (await UnitOfWork.DesignationMenuItems.Exists(x => x.DesignationId == MappingData.DesignationId && x.MenuId == MappingData.MenuId))
            return BadRequest(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "Mapping Already Exists.",
                Success = false,
                ReturnCode = StatusCodes.Status400BadRequest.ToString()
            });

        var returnData = await UnitOfWork.DesignationMenuItems.MapDesignationWithMenu(MappingData);
        if (!returnData)
            return BadRequest(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "Mapping Failed.",
                Success = true,
                ReturnCode = StatusCodes.Status500InternalServerError.ToString()
            });

        return Ok(new ResponseModel()
        {
            ReturnId = string.Empty,
            Message = "Mapping Done Successfully.",
            Success = true,
            ReturnCode = StatusCodes.Status201Created.ToString()
        });
    }

    [HttpGet("permitetdmenus/{designationid}")]    
    public async Task<ActionResult<List<DesignationMenuItemsDetailsDTO>>> PermitetdMenus(int designationid)
    {
        var allmappings = await UnitOfWork.DesignationMenuItems.GetAllMappings();
        if (allmappings == null)
            return NotFound(new ResponseModel()
            {
                ReturnId = string.Empty,
                Message = "No Data Found.",
                Success = false,
                ReturnCode = StatusCodes.Status404NotFound.ToString()
            });
        return Ok(allmappings.Where(x => x.DesignationId == designationid));
    }
    #endregion

}
