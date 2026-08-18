using edumis.DataAccess.IRepositories.ILeave;
using edumis.Models.Leave;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Leave;

public class LeaveRegisterRepo(ApplicationDBContext dbContext)
    : Repository<LeaveRegisterModel>(dbContext), ILeaveRegisterRepo {
    private readonly ApplicationDBContext dbContext = dbContext;

    public async Task<(bool, string)> AddLeaves(string employeeId, List<SingleAddLeaveDto> leaves, string actorId, string? ip = null) {

        var register = await dbContext.LeaveRegister.FirstOrDefaultAsync(r => r.EmployeeId == employeeId);
        if(register == null ) 
            return (false, "Missing leave register");
        if (leaves.GroupBy(x => x.LeaveType).Any(g => g.Count() > 1))
            return (false, "Duplicate leave types are not allowed.");
        
        foreach (var leave in leaves)  {
            var days = leave.Days;
            if(days<=0) return (false, "Invalid days to add");
            switch (leave.LeaveType){
                case LeaveType.CasualLeave:
                    register.CasualLeave += days; 
                    break;
                case LeaveType.SpecialCasualLeave:
                    register.SpecialCasualLeave += days; 
                    break;
                case LeaveType.ChildCareLeave :
                    register.ChildCareLeave += days; 
                    break;
                case LeaveType.HalfPayLeave :
                    register.HalfPayLeave += days; 
                    break;
                case LeaveType.MaternityLeave :
                    register.MaternityLeave += days; 
                    break;
                case LeaveType.PaternityLeave :
                    register.PaternityLeave += days; 
                    break;
                case LeaveType.EarnedLeave :
                    register.EarnedLeave += days; 
                    break;
            
                default: return (false, "Unsupported leave type");
            }
            
            dbContext.LeaveRegisterTrack.Add(new LeaveRegisterTrackModel {
                EmployeeId = employeeId,
                LeaveType = (int)leave.LeaveType,
                ActionAt = DateTime.UtcNow,
                ActionBy = actorId,
                ActionType = "Credit",
                Days = days,
                Comment = leave.Comment,
                IpAddress = ip
            });
        }
        
        await dbContext.SaveChangesAsync();
        return (true, "success");
    }

    public async Task<(bool, string)> DeductLeave(string employeeId, LeaveType leaveType, int days, string actorId, string applicationId, string? comment, string? ip = null){
        var register = await dbContext.LeaveRegister.FirstOrDefaultAsync(r => r.IsActive && r.EmployeeId == employeeId);
        if(register == null ) return (false, "Missing leave register");

        switch (leaveType){
            case LeaveType.CasualLeave: 
                register.CasualLeave -= days; 
                break;
            case LeaveType.HalfCasualLeave: 
                register.CasualLeave -= days/2f; 
                break;
            case LeaveType.SpecialCasualLeave: 
                register.SpecialCasualLeave -= days;
                break;
            case LeaveType.ChildCareLeave : 
                register.ChildCareLeave -= days;
                break;
            case LeaveType.HalfPayLeave : 
                register.HalfPayLeave -= days;
                break;
            case LeaveType.MaternityLeave : 
                register.MaternityLeave -= days;
                break;
            case LeaveType.PaternityLeave : 
                register.PaternityLeave -= days;
                break;
            case LeaveType.EarnedLeave : 
                register.EarnedLeave -= days;
                break;
            case LeaveType.CommutedHalfPayLeave : 
                register.HalfPayLeave -= days*2;
                break;
            case LeaveType.ExtraordinaryLeave :
            case LeaveType.SpecialLeave: 
                break;

            default: return (false, "Unsupported leave type");
        }
        
        if(register.CasualLeave<0 || register.HalfPayLeave < 0 || register.EarnedLeave<0 || register.MaternityLeave < 0 ||
           register.PaternityLeave < 0 || register.ChildCareLeave<0 || register.SpecialCasualLeave<0)
            return (false, "Insufficient leaves");
            
        
        dbContext.LeaveRegisterTrack.Add(new LeaveRegisterTrackModel {
            EmployeeId = employeeId,
            LeaveType = (int)leaveType,
            ActionAt = DateTime.UtcNow,
            ActionBy = actorId,
            ActionType = "Debit",
            Days = days,
            LeaveApplicationId = applicationId,
            Comment = comment,
            IpAddress = ip
        });
        
        await dbContext.SaveChangesAsync();
        return (true, "Success");

    }

}
