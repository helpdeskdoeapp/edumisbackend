using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface IMeetingResolutionsRepo : IRepository<MeetingResolutionsModel>
{
    Task<bool> CloseResolution(CloseMeetingResolutionRequestDTO requestDto, string userId);
    Task<List<MeetingResolutionDetailsDTO>?> GetResolutionList(string BranchId, DateOnly FromDate, DateOnly ToDate);
    Task<MeetingResolutionDetailsDTO?> ResolutionDetails(Guid ResolutionId);
    Task UpdateResolutionActualCost(Guid ResolutionId, decimal ActualCost, string UserId);
}
