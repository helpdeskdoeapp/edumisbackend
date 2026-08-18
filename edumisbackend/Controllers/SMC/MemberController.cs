using System.Security.Claims;
using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.SMC;

[Route("smc/[controller]")]
[ApiController]
[Authorize]
public class MemberController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfiguration configuration;

    public MemberController(IUnitOfWork UnitOfWork, IConfiguration configuration)
    {
        unitOfWork = UnitOfWork;
        this.configuration = configuration;
    }

    #region Validation APIs

    [HttpPost("checkmember/{mobileno}")]
    public async Task<IActionResult> CheckSMCMemberContact([FromRoute] string mobileno)
    {
        if (mobileno.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile Number.", StatusCodes.Status406NotAcceptable));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status500InternalServerError));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails =
            await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
                x.UserId == new Guid(BranchUserId) && x.IsValid == true);
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));

        var MemberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x =>
            x.MobileNo == mobileno &&
            x.BranchId == BranchLoginDetails.BranchId &&
            x.IsActive == true &&
            x.ForSession == currentSession.ForSession);
        if (MemberDetails == null)
            return Ok(ResponseModel<string>.Failure( "Member Not Found.", StatusCodes.Status404NotFound));

        return Ok(ResponseModel<object?>.Success(null, "Member Already Registered.", StatusCodes.Status409Conflict));
    }

    [HttpPost("checksmcemployee/{employeeid}")]
    public async Task<IActionResult> CheckSMCTeamEmployee([FromRoute] string employeeid) //[FromRoute] string branchid
    {
        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!" ));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails =
            await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
                x.UserId == new Guid(BranchUserId) && x.IsValid == true);
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));

        var MemberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x =>
            x.UniqueId == employeeid &&
            x.BranchId == BranchLoginDetails.BranchId &&
            x.IsActive == true &&
            x.ForSession == currentSession.ForSession);
        if (MemberDetails == null)
            return Ok(ResponseModel<string>.Failure("Employee Not Found.", StatusCodes.Status404NotFound));
        
        return Ok(ResponseModel<object?>.Success(null, "Employee Already Registered.", StatusCodes.Status409Conflict));
    }

    #endregion

    #region Create SMC Member

    [HttpPost("add")]
    public async Task<IActionResult> CreateMember([FromBody] SMCMemberRequestDTO memberDetails)
    {
        if (memberDetails == null)
            return Ok(ResponseModel<string>.Failure("Invalid Details"));

        if (memberDetails.MobileNo.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile Number.",StatusCodes.Status406NotAcceptable));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if(currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails =
            await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
                x.UserId == new Guid(BranchUserId) && x.IsValid == true);
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));
        
        var memberType = memberDetails.MemberType.AsEnumOrNull<SMCMemberType>();
        if(memberType==null)
            return Ok(ResponseModel<string>.Failure( "Invalid member type.", StatusCodes.Status403Forbidden));
            
        var membersList = (await unitOfWork.SMCMemberRegistrationsRepo
            .GetAll(x =>
                x.BranchId == BranchLoginDetails.BranchId &&
                x.ForSession == currentSession.ForSession)).ToList();
        
        var mobileExists = membersList.Any(x => x.MobileNo == memberDetails.MobileNo);
        if (mobileExists)
            return Ok(ResponseModel<string>.Failure( "Member with same mobile number already exists.", StatusCodes.Status403Forbidden));

        if (memberType.ExistsIn(SMCMemberType.STAFF_MEMBER, SMCMemberType.CHAIRPERSON)) {
           var empExists = membersList.Any(x => x.UniqueId == memberDetails.UniqueId);
           if (empExists)
                return Ok(ResponseModel<string>.Failure("Staff member with same employee id already exists.", StatusCodes.Status403Forbidden));
        }

        switch (memberType) {
            case SMCMemberType.CHAIRPERSON: {
                var chairpersonExists = membersList.Any(x => x is { IsActive: true, MemberType: (int)SMCMemberType.CHAIRPERSON });
                if (chairpersonExists)
                    return Ok(ResponseModel<string>.Failure("Chairperson already exists.",
                        StatusCodes.Status403Forbidden));
                break;
            }
            case SMCMemberType.VICE_CHAIRPERSON: {
                var chairpersonExists = membersList.Any(x => x is { IsActive: true, MemberType: (int)SMCMemberType.VICE_CHAIRPERSON });
                if (chairpersonExists)
                    return Ok(ResponseModel<string>.Failure("Vice chairperson already exists",
                        StatusCodes.Status403Forbidden));
                break;
            }
        }

        MemberRegistrationsModel saveObj = new MemberRegistrationsModel()
        {
            ForSession = currentSession.ForSession,
            MobileNo = memberDetails.MobileNo,
            UniqueId = string.IsNullOrEmpty(memberDetails.UniqueId) ? memberDetails.MobileNo : memberDetails.UniqueId,
            Name = memberDetails.Name,
            Gender = memberDetails.Gender,
            DesignationId = memberDetails.DesignationId,
            BranchId = BranchLoginDetails.BranchId,
            MemberType = memberDetails.MemberType,
            IsActive = true,
            CreatedBy = BranchUserId,
            ModifiedBy = BranchUserId
        };

        await unitOfWork.SMCMemberRegistrationsRepo.Add(saveObj);
        await unitOfWork.Save();

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(BranchUserId);
        if (branchDetails != null)
        {
            SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, "New member added",
                $"{memberDetails.Name} has been added to the SMC for {branchDetails.BranchName} ({branchDetails.BranchId}).");
        }
        return Ok(ResponseModel<string>.Success(saveObj.MemberId.ToString(), "Member Added Successfully.",StatusCodes.Status201Created));
    }

    #endregion

    #region Update SMC Member

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMember([FromBody] SMCMemberUpdateRequestDTO dto)
    {
        if (dto == null)
            return Ok(ResponseModel<string>.Failure( "Invalid Details"));

        if (dto.MobileNo.Length != 10)
            return Ok(ResponseModel<string>.Failure( "Invalid Mobile Number.", StatusCodes.Status406NotAcceptable));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var BranchLoginDetails =
            await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
                x.UserId == new Guid(BranchUserId) && x.IsValid == true);
        if (BranchLoginDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));

        var memberData =
            await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x =>
                x.MemberId == new Guid(dto.MemberId));
        if (memberData == null)
            return Ok(ResponseModel<string>.Failure( "Member Details Not Found.", StatusCodes.Status404NotFound));

        var memberType = dto.MemberType.AsEnumOrNull<SMCMemberType>();
        if(memberType==null)
            return Ok(ResponseModel<string>.Failure( "Invalid member type.", StatusCodes.Status403Forbidden));

        var membersList = (await unitOfWork.SMCMemberRegistrationsRepo.GetAll(x =>
            x.BranchId == BranchLoginDetails.BranchId && x.ForSession == currentSession.ForSession)).ToList();
        
        if (dto.MobileNo != memberData.MobileNo) {
            var mobileExists = membersList.Any(x => x.MobileNo == dto.MobileNo);
            if (mobileExists)
                return Ok(ResponseModel<string>.Failure("Member with same mobile number already exists."));
        }

        if ( memberType.ExistsIn(SMCMemberType.STAFF_MEMBER, SMCMemberType.CHAIRPERSON) && dto.UniqueId != memberData.UniqueId) {
            var employeeExists = membersList.Any(x => x.UniqueId == dto.UniqueId);
            if (employeeExists)
                return Ok(ResponseModel<string>.Failure("Staff member with same employee id already exists.", StatusCodes.Status403Forbidden));
        }

        if (await unitOfWork.SMCMemberRegistrationsRepo.UpdateMember(dto, BranchLoginDetails.BranchId, BranchUserId))
            return Ok(ResponseModel<string>.Success(dto.MemberId, "Member Updated Successfully.", StatusCodes.Status201Created ));

        return Ok(ResponseModel<string>.Failure("Failed to update member details!", StatusCodes.Status500InternalServerError));
    }

    #endregion

    #region Disable Member

    [HttpPost("activation/{memberid}/{status}")]
    public async Task<IActionResult> StatusUpdate([FromRoute] string memberid, [FromRoute] bool status)
    {
        var tokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string branchUserId = tokenParam != null ? edumis.Common.Utilities.DecryptString(tokenParam) : string.Empty;

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));
        
        var memberData = await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x => x.MemberId == new Guid(memberid));
        if (memberData == null) return 
            Ok(ResponseModel<string>.Failure("No Details Found.", StatusCodes.Status404NotFound));
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var loggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        var res = await unitOfWork.SMCMemberRegistrationsRepo.UpdateStatus(memberid, status, loggedInUserId);
        if (res)
        {
            var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(branchUserId);
            if (branchDetails != null)
            {
                var action = status ? "activated" : "deactivated";
                SmcAppNotifier.SendNotificationSilently(branchDetails.BranchId, $"Member {action}",
                    $"{memberData.Name} has been {action} in the SMC for {branchDetails.BranchName} ({branchDetails.BranchId}).");
            }

            return Ok(ResponseModel<object?>.Success(null, "Member status updated successfully."));
        }
        return Ok(ResponseModel<string>.Failure( "Failed to update member status!", StatusCodes.Status500InternalServerError ));
        
    }

    #endregion

    #region Get Member Details

    [HttpGet("memberdetails/{memberid}")]
    public async Task<IActionResult> GetMemberDetails([FromRoute] string memberid)
    {
        if (string.IsNullOrEmpty(memberid) || memberid == "")
        {
            return Ok(ResponseModel<string>.Failure( "Invalid Data Input!."));
        }

        var EmpDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetMemberDetails(memberid);

        if (EmpDetails == null)
            return Ok(ResponseModel<string>.Failure( "No Data Found.", StatusCodes.Status404NotFound));

        return Ok(EmpDetails);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchMember([FromBody] SearchSMCTeamMembers searchMember)
    {
       
        if (string.IsNullOrEmpty(searchMember.BranchId))
        {
            var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string BranchUserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;
            var BranchLoginDetails =
                await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
                    x.UserId == new Guid(BranchUserId) && x.IsValid == true);
            if (BranchLoginDetails == null)
                return Ok(ResponseModel<string>.Failure("Branch Details Not Found!", StatusCodes.Status404NotFound));
            searchMember.BranchId = BranchLoginDetails.BranchId;
        }
        if (searchMember.ForSession == null)
        {
            var session = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
            if (session == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                    StatusCodes.Status204NoContent));
            searchMember.ForSession = session.ForSession;
        }
        var memberDetails = await unitOfWork.SMCMemberRegistrationsRepo.SearchMembers(searchMember);

        if (memberDetails == null || memberDetails.Count == 0)
            return Ok(ResponseModel<string>.Failure( "No Data Found.", StatusCodes.Status404NotFound));

        return Ok(memberDetails);
    }

    [HttpGet("allmembers_against_mobileno/{activeonly?}")]
    public async Task<IActionResult> GetAllMembersAgainstMobileNo([FromRoute] bool? activeonly = true)
    {
        var TokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string MemberId = TokenId != null ? edumis.Common.Utilities.DecryptString(TokenId) : string.Empty;

        var memberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x => x.MemberId == new Guid(MemberId));
        if(memberDetails == null)
            return Ok(ResponseModel<string>.Failure( "Member Details Not Found.", StatusCodes.Status404NotFound));

        var AllMembersData = await unitOfWork.SMCMemberRegistrationsRepo.GetAllMembers(memberDetails.MobileNo);
        if (AllMembersData == null)
            return Ok(ResponseModel<string>.Failure( "Member Details Not Found.", StatusCodes.Status404NotFound));

        return Ok(activeonly == true
            ? AllMembersData.Where(x => x.IsActive == true).OrderBy(x => x.BranchId)
            : AllMembersData);
    }

    #endregion
}