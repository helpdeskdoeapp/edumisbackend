using System.Security.Claims;
using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumis.Models.Users;
using edumisbackend.Common;
using Microsoft.AspNetCore.Mvc;

namespace edumisbackend.Controllers.SMC;

[Route("api/[controller]")]
[ApiController]
public class SmcAdminController(IUnitOfWork unitOfWork): ControllerBase {
    
    [HttpGet("budget")]
    public async Task<IActionResult> GetAllocations([FromQuery] string? session, [FromQuery] string? schoolId) {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());
        
        var noSession = session.IsNullOrBlank();
        var noSchool = schoolId.IsNullOrBlank();
        
        var allocations = await unitOfWork.SmcBudgetRepo.GetAll(b => (noSession || b.Session == session) &&
                                                                      (noSchool || b.SchoolId == schoolId));
        
        return Ok(ResponseModel<List<SmcBudgetAllocationModel>>.Success(allocations.ToList() ));
    }
    
    [HttpPost("budget")]
    public async Task<IActionResult> BulkAllocation([FromBody] List<SmcBudgetNewAllocationDto> requests) {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());

        if (requests.Any(r => r.Amount < 0)) {
            return Ok(ResponseModel<string>.Failure("Allocation amount can't be negative", StatusCodes.Status406NotAcceptable ));
        }

        
        string? currentSession = null;
        if (requests.Any(r => r.Session == null)) {
            currentSession = (await unitOfWork.AcademicSessions
                .GetFirstOrDefault(x => x.IsValid == true && x.IsCurrent == true))?.ForSession;
            if (currentSession == null)
                return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!", StatusCodes.Status204NoContent ));
        }

        var allocations = requests.Select(req => new SmcBudgetAllocationHistoryModel {
            Session = req.Session ?? currentSession!,
            SchoolId = req.SchoolId,
            Amount = req.Amount,
            AllocationType = req.AllocationType,
            AllocationDate = req.AllocationDate ?? DateTime.UtcNow,
            Remarks = req.Remarks,
            CreatedBy = userId
        }).ToList();
        
        await unitOfWork.SmcBudgetHistoryRepo.AddRange(allocations);
        foreach (var allocation in allocations) 
            await unitOfWork.SmcBudgetRepo.AddOrUpdateAllocation(allocation.Session, allocation.SchoolId, allocation.Amount, userId);
        
        await unitOfWork.Save();
        
        return Ok(ResponseModel<string?>.Success(null, "Budget allocated.", StatusCodes.Status201Created ));
    }
    
    [HttpPost("budget/revoke/{rowId}")]
    public async Task<IActionResult> RevokeAllocation([FromRoute] long rowId) {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());

        var tracker = await unitOfWork.SmcBudgetHistoryRepo.GetFirstOrDefault(h=>h.RowId==rowId);
        
        if(tracker is null)
            return Ok(ResponseModel<string>.Failure("Budget transaction not found", StatusCodes.Status406NotAcceptable ));
            
        var entry = await unitOfWork.SmcBudgetRepo.GetFirstOrDefault(b=>b.Session==tracker.Session && b.SchoolId==tracker.SchoolId);
        if(entry is null)    
            return Ok(ResponseModel<string>.Failure("Budget allocation (School with session) not found", StatusCodes.Status406NotAcceptable ));

        entry.Allocation -= tracker.Amount;
        entry.ModifiedDate = DateTime.UtcNow;
        entry.ModifiedBy = userId;
        await unitOfWork.SmcBudgetHistoryRepo.Add(new SmcBudgetAllocationHistoryModel{
            Session = tracker.Session,
            SchoolId = tracker.SchoolId,
            Amount = -tracker.Amount,
            AllocationType = (int)AllocationType.Revocation,
            AllocationDate = DateTime.UtcNow,
            Remarks = $"Budget revocation for transaction #{rowId}",
            CreatedBy = userId,
            CreatedDate = DateTime.UtcNow
        });
        
        await unitOfWork.Save();
        
        return Ok(ResponseModel<string?>.Success(null, "Budget allocated.", StatusCodes.Status201Created ));
    }

    
    [HttpGet("branches")]
    public async Task<IActionResult> GetAllBranchAccounts() {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());
        
        var accounts = await unitOfWork.SMCUserRepo.GetAll();
        var users = accounts.Select(a=>a.ToSmcUserDto()).ToList();
        
        return Ok(ResponseModel<List<SmcUserDto>>.Success(users));
    }
    
    [HttpGet("branches/{branchGuid}")]
    public async Task<IActionResult> GetBranchAccountDetails([FromRoute] string branchGuid) {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());
        
        var account = await unitOfWork.SMCUserRepo.GetFirstOrDefault(u=>u.UserId== new Guid(branchGuid));
        if(account is null)
            return Ok(ResponseModel<object?>.Failure("User Not Found",  StatusCodes.Status404NotFound ));
        
        return Ok(ResponseModel<SMCAccountsModel>.Success(account));
    }

    // [HttpPost("branches")]
    // public async Task<IActionResult> CreateBranchAccount([FromBody] SmcUserCreateDto dto) {
    //     
    // }
    
    [HttpPut("branches/{branchGuid}")]
    public async Task<IActionResult> UpdateBranchAccount([FromRoute] string branchGuid, [FromBody] SmcUserUpdateDto dto) {
        var userId = GetUserId();
        if (userId is null || !IsAdmin)
            return Ok(ResponseModel<object?>.Unauthorized());
        
            
        var user = await unitOfWork.SMCUserRepo.GetFirstOrDefault(u=>u.UserId== new Guid(branchGuid));
        if(user is null)
            return Ok(ResponseModel<object?>.Failure("User Not Found",  StatusCodes.Status404NotFound ));
        
        if(dto.UserType is null && dto.EmailId is null && dto.MobileNo is null && dto.Photo is null && dto.IsAccountLocked is null && dto.IsValid is null)
            return Ok(ResponseModel<object?>.Failure("Nothing to update" ));
        
        user.UserType = dto.UserType ?? user.UserType;
        user.EmailId = dto.EmailId ?? user.EmailId;
        user.MobileNo = dto.MobileNo ?? user.MobileNo;
        user.Photo = dto.Photo ?? user.Photo;
        user.IsAccountLocked = dto.IsAccountLocked ?? user.IsAccountLocked;
        user.IsValid = dto.IsValid ?? user.IsValid;
        
        user.ModifiedBy = userId;
        user.ModifiedDate = DateTime.UtcNow;
        
        await unitOfWork.Save();
        
        return Ok(ResponseModel<object?>.Success(null, "User updated successfully"));
        
    }

    private bool IsAdmin =>
        // int.TryParse(User.FindFirstValue("UserType"), out var userType) &&
        // userType == (int)UserType.SysAdmin &&
        int.TryParse(User.FindFirstValue("UserRole"), out var userRole) &&
        userRole == (int)UserRole.SysAdmin;

    private string? GetUserId() {
        var token = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return token is null ? null : Utilities.DecryptString(token);
    }

}


public class SmcUserCreateDto {
    public string BranchId = "";
    public int UserType;
    public string? EmailId;
    public string? MobileNo;
    public byte[]? Photo;
}

public class SmcUserUpdateDto {
    public int? UserType;
    public string? EmailId;
    public string? MobileNo;
    public byte[]? Photo;
    public bool? IsAccountLocked;
    public bool? IsValid;
}

public class SmcUserDto {
    public Guid UserId;
    public string BranchId = "";
    public string BranchName = "";
    public int UserType;
    public string? UserTypeDesc;
    public string? EmailId;
    public string? MobileNo;
    public byte[]? Photo;
    public bool? IsValid;
    public bool? IsAccountLocked;
}



public static class SmcAccountMapExtension {
    public static SmcUserDto ToSmcUserDto(this SMCAccountsModel model) =>
        new() {
            UserId = model.UserId,
            BranchId = model.BranchId,
            UserType = model.UserType,
            EmailId = model.EmailId,
            MobileNo = model.MobileNo,
            Photo = model.Photo,
            IsValid = model.IsValid,
            IsAccountLocked = model.IsAccountLocked,
        };
}

public enum AllocationType {
    Budget = 2301,
    Donation = 2302,
    Revocation = 2303
}