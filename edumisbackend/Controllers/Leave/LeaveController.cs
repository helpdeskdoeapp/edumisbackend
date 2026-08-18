using System.Security.Claims;
using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models.Employees.DTO;
using edumis.Models.Leave;
using edumis.Models.Masters;
using edumis.Models.Masters.DTO;
using edumis.Models.Users;
using edumis.Models.Users.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.Leave;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeaveController(IUnitOfWork unitOfWork) : ControllerBase
{
    #region Employee routes

    [HttpPost("apply")]
    public async Task<ActionResult> ApplyForLeave([FromBody] LeaveApplicationRequestDto requestDto)
    {
        var loginUser = await GetLoginUser();
        if (loginUser == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        if (requestDto.EmployeeId != loginUser.UniqueId)
            return Ok(ResponseModel<string>.Unauthorized("Access denied"));

        var userType = loginUser.UserType.AsEnumOrNull<UserType>();
        if(userType.NotIn(UserType.Teacher,UserType.Executive ))
            return Ok(ResponseModel<string>.Unauthorized("Application not allowed"));
        
        if (loginUser.BranchId == null)
            return NotFound(ResponseModel<string>.Failure("Reporting branch not found"));
        
        if(requestDto.LeaveType != (int)LeaveType.CasualLeave)
            requestDto.Days = requestDto.ToDate.DayNumber - requestDto.FromDate.DayNumber +1;
        
        var (success, message) = await ValidateLeaveRequest(requestDto);
        if(!success) 
            return BadRequest(ResponseModel<string>.Failure(message??"Invalid request")); 

        BranchesNamesDTO? diverted = null,
            service = null,
            zone = null,
            district = null,
            region = null,
            goc = null,
            dir = null;
        var approvers = GetApprovalChain(requestDto);

        if (approvers.Contains(LeaveLevel.DivertedBranch)) {
            diverted = null; // todo(amit) get diverted branch from employee details
        }

        if (approvers.Contains(LeaveLevel.ServiceBranch))        {
            var branch = await unitOfWork.BranchRepo.GetDetails(loginUser.BranchId);
            if (branch == null)
                return BadRequest(ResponseModel<string>.Failure("Invalid branch"));
            service = new BranchesNamesDTO()            {
                BranchId = branch.BranchId,
                BranchType = branch.BranchType,
                BranchName = branch.BranchName
            };
        }

        if (approvers.Contains(LeaveLevel.Zone))
        {
            var b = await unitOfWork.BranchRepo.GetDetails(loginUser.BranchId);
            if (b?.BranchType == (int)BranchType.School)
            {
                zone = await unitOfWork.BranchRepo.GetParentBranch(loginUser.BranchId);
                if (zone?.BranchType != (int)BranchType.Zone)
                    zone = null;
            }
        }

        if (approvers.Contains(LeaveLevel.District) && zone != null)
        {
            district = await unitOfWork.BranchRepo.GetParentBranch(zone.BranchId);
            if (district?.BranchType != (int)BranchType.District)
                district = null;
        }

        if (approvers.Contains(LeaveLevel.Region) && district != null)
        {
            region = await unitOfWork.BranchRepo.GetParentBranch(district.BranchId);
            if (region?.BranchType != (int)BranchType.Region)
                region = null;
        }

        if (approvers.Contains(LeaveLevel.Goc))
        {
            goc = new BranchesNamesDTO
            {
                BranchId = "5000003",
                BranchName = "DTE-Administration-1 : GOC",
                BranchType = (int)BranchType.Branch
            };
        }

        if (approvers.Contains(LeaveLevel.HqBranch))
        {
            dir = new BranchesNamesDTO
            {
                BranchId = "500000",
                BranchName = "DTE-Office of Director of Education",
                BranchType = (int)BranchType.HQ
            };
        }

        var application = new LeaveApplicationModel
        {
            EmployeeId = requestDto.EmployeeId,
            LeaveType = requestDto.LeaveType,
            Days = requestDto.Days,
            FromDate = requestDto.FromDate,
            ToDate = requestDto.ToDate,
            Reason = requestDto.Reason,
            AppliedAt = DateTime.UtcNow,
            LeaveStation = requestDto.LeaveStation,

            LeaveWithNoc = requestDto.NocNeeded,
            ChildDob = requestDto.ChildDob,
            AddressDuringLeave = requestDto.Address,

            DivertedBranchId = diverted?.BranchId,
            ServiceBranchId = service?.BranchId,
            ZoneId = zone?.BranchId,
            DistrictId = district?.BranchId,
            RegionId = region?.BranchId,
            GocId = goc?.BranchId,
            HqBranchId = dir?.BranchId,

            LeaveStatus = LeaveStatus.Pending,
            CurrentLevel = diverted != null ? LeaveLevel.DivertedBranch : LeaveLevel.ServiceBranch,
            
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = requestDto.EmployeeId
        };

        await unitOfWork.LeaveApplicationRepo.Add(application);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success("Successfully applied"));
    }

    [HttpPost("applications/withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] ProcessLeaveDto dto) => await ProcessApplication(dto);

    [HttpGet("applications")]
    public async Task<IActionResult> GetApplications() => await GetApplicationsAtEmpl();

    [HttpGet("applications/{id}")]
    public async Task<IActionResult> GetApplications([FromRoute] string id) => await GetApplicationsAtEmpl(id);

    [HttpGet("applications/{applicationId}/track")]
    public async Task<IActionResult> TrackApplications([FromRoute] string applicationId)
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var application =
            await unitOfWork.LeaveApplicationRepo.GetFirstOrDefault(a => a.ApplicationId == applicationId);
        if (application == null)
            return BadRequest("No application found");

        
        if (user.UniqueId != application.EmployeeId)
            return Ok(ResponseModel<string>.Unauthorized("Access denied"));

        var nodes = await unitOfWork.LeaveApplicationRepo.TrackApplication(application);
        if(nodes==null)
            return NotFound(ResponseModel<string>.Failure("Invalid employee details"));

        if (nodes.Count == 0) 
            return NotFound(ResponseModel<string>.Failure("No tracking history"));
        
        return Ok(ResponseModel<List<LeaveApplicationTrackDto>>.Success(nodes));
    }

    [HttpGet("balance")]
    public async Task<ActionResult> GetAvailableLeaves()
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(user.UniqueId);
        if (employee == null)
            return BadRequest(ResponseModel<string>.Failure("Employee not found"));

        if (!user.UniqueId.ExistsIn(employee.EmployeeId, employee.CurrentBranchID))
            return Ok(ResponseModel<string>.Unauthorized("access denied"));

        var balance = await GetLeaveBalance(employee.EmployeeId);
        return Ok(ResponseModel<LeaveBalanceDto>.Success(balance));
    }

    [HttpGet("balance/track")]
    public async Task<ActionResult> TrackLeaveRegister([FromRoute] string employeeId)
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(employeeId);
        if (employee == null)
            return BadRequest(ResponseModel<string>.Failure("Employee not found"));

        if (!user.UniqueId.ExistsIn(employeeId, employee.CurrentBranchID))
            return Ok(ResponseModel<string>.Unauthorized("access denied"));
        
        var list = await unitOfWork.LeaveRegisterTrackRepo.GetAll(r => r.EmployeeId == employeeId);
        var nodes = list.Select(t => new LeaveBalanceTrackDto {
            LeaveType = t.LeaveType,
            ActionBy = t.ActionBy,
            ActionType = t.ActionType,
            ActionAt = t.ActionAt,
            Days = t.Days,
            LeaveApplicationId = t.LeaveApplicationId,
            Comment = t.Comment
        }).ToList();
        return Ok(ResponseModel<List<LeaveBalanceTrackDto>>.Success(nodes));
    }

    #endregion

    #region Branch routes

    [HttpPost("branch/process")]
    public async Task<IActionResult> ProcessApplication([FromBody] ProcessLeaveDto dto)
    {
        var loginUser = await GetLoginUser();
        if (loginUser == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var actorId = loginUser.UniqueId;
        if (!Enum.TryParse(dto.Action, true, out LeaveAction action))
            return BadRequest(ResponseModel<string>.Failure("Invalid action"));

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        var (done, message) = await ProcessApplication(dto.ApplicationId, action, actorId, ip, dto.Comment);
        if (!done)
            return BadRequest(ResponseModel<string>.Failure(message));

        return Ok(ResponseModel<string>.Success($"{action.ToString()} successful"));
    }

    [HttpGet("branch/balance/{employeeId}")]
    public async Task<ActionResult> GetAvailableLeavesForEmployee([FromRoute] string employeeId)
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(employeeId);
        if (employee == null)
            return BadRequest(ResponseModel<string>.Failure("Employee not found"));

        if (user.UniqueId != employee.CurrentBranchID)
            return Ok(ResponseModel<string>.Unauthorized("access denied"));

        var balance = await GetLeaveBalance(employee.EmployeeId);
        return Ok(ResponseModel<LeaveBalanceDto>.Success(balance));
    }

    [HttpPut("branch/balance")]
    public async Task<ActionResult> AddLeave([FromBody] AddLeaveDto leaveDto)
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("Invalid login"));

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(leaveDto.EmployeeId);
        if (employee == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid employee"));

        if (user.UniqueId != employee.CurrentBranchID)
            return BadRequest(ResponseModel<string>.Failure("Employee not serviceable by this office"));

        if (leaveDto.Leaves.Any(leave => !Enum.IsDefined(leave.LeaveType)))
            return BadRequest("Invalid leave type.");

        var register = await unitOfWork.LeaveRegisterRepo.GetFirstOrDefault(r => r.EmployeeId == leaveDto.EmployeeId);
        if (register == null)
        {
            // create new register entry for the employee
            await unitOfWork.LeaveRegisterRepo.Add(new LeaveRegisterModel { EmployeeId = employee.EmployeeId });
            await unitOfWork.Save();
        }

        var (success, message) =
            await unitOfWork.LeaveRegisterRepo.AddLeaves(leaveDto.EmployeeId, leaveDto.Leaves, user.UniqueId);
        if (!success)
            return BadRequest(ResponseModel<string>.Failure(message));
        await unitOfWork.Save();

        register = await unitOfWork.LeaveRegisterRepo.GetFirstOrDefault(r => r.EmployeeId == leaveDto.EmployeeId);
        return Ok(ResponseModel<LeaveRegisterModel>.Success(register!));
    }

    [HttpGet("branch/applications")]
    public async Task<IActionResult> BranchAllApplications([FromQuery] string status = "all") => await GetApplicationsAtBranch(status);

    [HttpGet("branch/application/{applicationId}")]
    public async Task<IActionResult> BranchApplicationDetails([FromRoute] string applicationId) =>
        await GetApplicationsAtBranch("all", applicationId);
    

    [HttpGet("branch/employees")]
    public async Task<IActionResult> GetBranchWiseEmployees()
    {
        var user = await GetLoginUser();
        if (user == null) 
            return Ok(ResponseModel<string>.Unauthorized("Invalid login"));
        
        var branchEmps = await unitOfWork.EmployeesRepo.GetEmployeesByBranch(user.BranchId);

        return Ok(ResponseModel<List<EmployeeBasicDto>>.Success(branchEmps.ToList()));
    }


    [HttpGet("branch/applications/{applicationId}/track")]
    public async Task<IActionResult> GetBranchApplicationTrack([FromRoute] string applicationId) {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        var application =
            await unitOfWork.LeaveApplicationRepo.GetFirstOrDefault(a => a.ApplicationId == applicationId);
        if (application == null)
            return BadRequest("No application found");

        string?[] stakeholders = [application.DivertedBranchId, application.ServiceBranchId,
            application.ZoneId, application.DistrictId, application.GocId, application.HqBranchId];
        if (!user.UniqueId.ExistsIn(stakeholders))
            return Ok(ResponseModel<string>.Unauthorized("Access denied"));

        var nodes = await unitOfWork.LeaveApplicationRepo.TrackApplication(application);
        if(nodes==null)
            return NotFound(ResponseModel<string>.Failure("Invalid employee details"));

        if (nodes.Count == 0) 
            return NotFound(ResponseModel<string>.Failure("No tracking history"));
        
        return Ok(ResponseModel<List<LeaveApplicationTrackDto>>.Success(nodes));
    }

    #endregion

    #region Private functions

    private async Task<UserDTO?> GetLoginUser()
    {
        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(nameIdentifier))
            return null;
        var userGuid = Utilities.DecryptString(nameIdentifier);
        if (!Guid.TryParse(userGuid, out var parsedGuid))
            return null;
        return await unitOfWork.Users.GetUserDetails(parsedGuid);
    }

    private async Task<(bool, string)> ProcessApplication(string applicationId, LeaveAction action, string actorId,
        string? ip, string? comment) {
        var model = await unitOfWork.LeaveApplicationRepo.GetFirstOrDefault(a => a.ApplicationId == applicationId);
        if (model == null)
            return (false, "Invalid application");

        if (model.LeaveStatus != LeaveStatus.Pending)
            return (false, "Application is already finalized");

        if ((action != LeaveAction.Withdraw && actorId != unitOfWork.LeaveApplicationRepo.GetActionBranch(model)) ||
            (action == LeaveAction.Withdraw && actorId != model.EmployeeId))
            return (false, "Not authorized");

        var nextApprover = GetNextLevel(model);

        switch (action) {
            case LeaveAction.Withdraw:
                model.LeaveStatus = LeaveStatus.Withdrawn;
                break;

            case LeaveAction.Reject:
                model.LeaveStatus = LeaveStatus.Rejected;
                break;

            case LeaveAction.Forward:
                if (nextApprover == null)
                    return (false, "Application can only be approved at this level");
                model.CurrentLevel = (LeaveLevel)nextApprover;
                break;

            case LeaveAction.Approve:
                if (nextApprover != null)
                    return (false, "Application can only be forwarded, not approved at this level");
                model.LeaveStatus = LeaveStatus.Approved;
                var (success, message) = await unitOfWork.LeaveRegisterRepo.DeductLeave(model.EmployeeId,
                    (LeaveType)model.LeaveType, model.Days, actorId, applicationId, comment, ip);
                if (!success)
                    return (false, message);
                break;
            default:
                return (false, "Invalid action");
        }

        await unitOfWork.LeaveApplicationTrackRepo.Add(new LeaveApplicationTrackModel
        {
            ApplicationId = applicationId,
            ActionBy = actorId,
            ActionType = action.ToString(),
            ActionAt = DateTime.UtcNow,
            IpAddress = ip,
            Comment = comment
        });
        await unitOfWork.Save();

        return (true, "Success");
    }

    private static LeaveLevel? GetNextLevel(LeaveApplicationModel model)
    {
        var level = model.CurrentLevel;
        switch (level)
        {
            case LeaveLevel.DivertedBranch:
                if (model.ServiceBranchId != null) return LeaveLevel.ServiceBranch;
                goto case LeaveLevel.ServiceBranch;

            case LeaveLevel.ServiceBranch:
                if (model.ZoneId != null) return LeaveLevel.Zone;
                goto case LeaveLevel.Zone;

            case LeaveLevel.Zone:
                if (model.HqBranchId != null) return LeaveLevel.District;
                goto case LeaveLevel.District;

            case LeaveLevel.District:
                if (model.RegionId != null) return LeaveLevel.Region;
                goto case LeaveLevel.Region;

            case LeaveLevel.Region:
                if (model.GocId != null) return LeaveLevel.Goc;
                goto case LeaveLevel.Goc;

            case LeaveLevel.Goc:
                if (model.HqBranchId != null) return LeaveLevel.HqBranch;
                break;
        }

        return null;
    }

    private bool IsFinalLevel(LeaveApplicationModel model) => GetNextLevel(model) == null;
    private List<LeaveLevel> GetApprovalChain(LeaveApplicationRequestDto requestDto)
    {
        // todo implement actual rules
        return [LeaveLevel.ServiceBranch];
    }

    private async Task<(bool, string?)> ValidateLeaveRequest(LeaveApplicationRequestDto dto) {
        if (dto.FromDate > dto.ToDate) 
            return (false, "From date should be before To date");
        
        if(dto.Days <= 0)
            return (false, "Number of days must be greater than zero");
        
        var leaveType = dto.LeaveType.AsEnumOrNull<LeaveType>();
        if (leaveType == null) 
            return (false, "Invalid LeaveType");
        
        var daysDiff = dto.ToDate.DayNumber -  dto.FromDate.DayNumber + 1;

        var daysError = leaveType switch {
            LeaveType.CasualLeave => dto.Days > daysDiff,
            LeaveType.HalfCasualLeave => dto.Days != 1,
            _ => dto.Days != daysDiff
        };
        if(daysError) return (false, "Invalid number of days");

        if (leaveType == LeaveType.HalfCasualLeave) {
            if(dto.FromDate != dto.ToDate)
                return (false, "For Half CL, From date should be same as the To date");
            if(dto.Days != 1)
                return (false, "Only a single Half CL can be applied at a time");
        }

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(dto.EmployeeId);
        if (employee == null)
            return (false, "invalid employee");
        if ((employee.Gender == 102 && leaveType == LeaveType.PaternityLeave) ||
            (employee.Gender == 101 && leaveType == LeaveType.MaternityLeave))
            return (false, "Leave type mismatch with gender");
            
        var applications = await unitOfWork.LeaveApplicationRepo
            .GetAll(l=>l.EmployeeId==dto.EmployeeId && l.LeaveStatus !=LeaveStatus.Rejected && l.LeaveStatus != LeaveStatus.Withdrawn);
        if (applications.Any(l => dto.FromDate <= l.ToDate && dto.ToDate >= l.FromDate))
            return (false, "Invalid date range requested");
            
        var balance = await unitOfWork.LeaveRegisterRepo.GetFirstOrDefault(r => r.IsActive && r.EmployeeId == dto.EmployeeId);
        if (balance == null && 
            leaveType.ExistsIn(LeaveType.CasualLeave, LeaveType.HalfCasualLeave, LeaveType.HalfPayLeave, 
                LeaveType.EarnedLeave, LeaveType.MaternityLeave, LeaveType.PaternityLeave))
            return (false, "Leave balance register not updated");
        
        if( leaveType switch { 
               LeaveType.CasualLeave => balance?.CasualLeave < dto.Days,
               LeaveType.HalfCasualLeave => balance?.CasualLeave < 0.5f,
               LeaveType.SpecialCasualLeave => balance?.SpecialCasualLeave < dto.Days,
               LeaveType.EarnedLeave => balance?.EarnedLeave < dto.Days,
               LeaveType.HalfPayLeave => balance?.HalfPayLeave < dto.Days,
               LeaveType.CommutedHalfPayLeave => balance?.HalfPayLeave < dto.Days*2,
               LeaveType.PaternityLeave => balance?.PaternityLeave < dto.Days,
               LeaveType.MaternityLeave => balance?.MaternityLeave < dto.Days,
               LeaveType.ChildCareLeave => balance?.ChildCareLeave < dto.Days,
               _ => false // pass for other leave types
           })
            return (false, "Insufficient leaves");
        
        return (true, null);
    }

    private async Task<IActionResult> GetApplicationsAtEmpl(
        string? applicationId = null)
    {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("User Details Not Found!"));

        if (((UserType)user.UserType).NotIn(UserType.Teacher, UserType.Executive))
        {
            return Ok(ResponseModel<string>.Unauthorized("access denied!"));
        }

        var employee = await unitOfWork.EmployeesRepo.GetEmployeeDetails(user.UniqueId);
        if (employee == null)
            return BadRequest(ResponseModel<string>.Failure("Employee not found"));

        if (user.UniqueId != employee.EmployeeId)
            return Ok(ResponseModel<string>.Unauthorized("access denied"));

        var apps = await unitOfWork.LeaveApplicationRepo.GetApplicationsAtEmpl(employee.EmployeeId, applicationId);

        if (applicationId == null)
            return Ok(ResponseModel<List<LeaveApplicationsAtEmplDto>>.Success(apps.ToList()));

        var a = apps.FirstOrDefault();
        if (a == null)
            return NotFound(ResponseModel<string>.Failure("Application not found"));


        return Ok(ResponseModel<LeaveApplicationsAtEmplDto>.Success(a));
    }
    
    private async Task<IActionResult> GetApplicationsAtBranch(string status, string? applicationId = null) {
        var user = await GetLoginUser();
        if (user == null)
            return Ok(ResponseModel<string>.Unauthorized("Invalid login"));

        var branch = await unitOfWork.BranchRepo.GetDetails(user.UniqueId);
        if (branch == null)
            return BadRequest(ResponseModel<string>.Failure("Invalid branch"));

        if (status.NotIn("pending", "approved", "rejected", "forwarded", "all"))
            return BadRequest(ResponseModel<string>.Failure("Invalid status requested"));

        var apps =
            await unitOfWork.LeaveApplicationRepo.GetApplicationsAtBranch(status, branch.BranchId,
                (BranchType)branch.BranchType, applicationId);
        
        if(applicationId == null)
            return Ok(ResponseModel<List<LeaveApplicationsAtBranchDto>>.Success(apps));
        
        var singleAppln = apps.FirstOrDefault();
        if (singleAppln == null)
            return NotFound(ResponseModel<string>.Failure("Application not found"));

        return Ok(ResponseModel<LeaveApplicationsAtBranchDto>.Success(singleAppln));
    }

    private async Task<LeaveBalanceDto> GetLeaveBalance(string employeeId) {
        var balance = await unitOfWork.LeaveRegisterRepo
            .GetFirstOrDefault(r => r.IsActive && r.EmployeeId == employeeId);

        return new LeaveBalanceDto {
            EmployeeId = employeeId,
            CasualLeave = balance?.CasualLeave??0,
            SpecialCasualLeave = balance?.SpecialCasualLeave??0,
            EarnedLeave = balance?.EarnedLeave??0,
            MaternityLeave = balance?.MaternityLeave??0,
            PaternityLeave = balance?.PaternityLeave??0,
            HalfPayLeave = balance?.HalfPayLeave??0,
            ChildCareLeave = balance?.ChildCareLeave??0,
        };
            
    }
    
    #endregion functions
}