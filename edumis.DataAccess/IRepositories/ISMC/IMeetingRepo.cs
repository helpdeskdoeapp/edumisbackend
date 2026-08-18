using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface IMeetingRepo : IRepository<MeetingModel> {
    Task<bool> Deactivate(string meetingId, string userId);    
    Task<bool> Update(MeetingUpdateRequestDTO meetingData, string userId);
    Task<bool> UpdatePostMeetingData(ConcludeMeetingRequestDTO meetingData, string userId);
    Task<MeetingDetailsDTO?> GetMeetingDetails(Guid meetingId);
    Task<List<MeetingsListDTO>?> GetMeetings(string branchId, string forSession);
    Task<List<MeetingsListDTO>?> GetMeetings(string branchId, string forSession, int status);
    Task<bool> ValidateMeetingAgendaSerialNos(Guid meetingId, int[] agendaSrNos);
}
