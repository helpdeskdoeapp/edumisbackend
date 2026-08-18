using edumis.Models.Leave;
using edumis.Models.Masters;

namespace edumis.DataAccess.IRepositories.ILeave;

public interface ILeaveApplicationRepo : IRepository<LeaveApplicationModel> {
    public Task<List<LeaveApplicationsAtBranchDto>> GetApplicationsAtBranch(string status, string branchId, BranchType branchType, string? applicationId = null);

    public Task<List<LeaveApplicationsAtEmplDto>> GetApplicationsAtEmpl(string emplId,
        string? applicationId = null);

    public string? BranchIdAtLevel(LeaveApplicationModel model, LeaveLevel? level);
    
    public string? GetActionBranch(LeaveApplicationModel model) => BranchIdAtLevel(model, model.CurrentLevel);

    public Task<List<LeaveApplicationTrackDto>?> TrackApplication(LeaveApplicationModel app);
}