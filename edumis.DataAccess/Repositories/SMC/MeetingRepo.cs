using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace edumis.DataAccess.Repositories.SMC;

internal class MeetingRepo(ApplicationDBContext dBContext) : Repository<MeetingModel>(dBContext), IMeetingRepo {
    private readonly ApplicationDBContext dBContext = dBContext;

    public async Task<bool> Deactivate(string meetingId, string userId) {
        await dBContext.SMCMeeting.Where(x => x.MeetingId == new Guid(meetingId)).ExecuteUpdateAsync(
            b=>b
            .SetProperty(prop => prop.Status, (int)SMCMeetingStatus.CANCELLED)
            .SetProperty(prop => prop.ModifiedBy, userId)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        return true;
    }

    public async Task<bool> Update(MeetingUpdateRequestDTO meetingData, string userId) {
        await dBContext.SMCMeeting.Where(x => x.MeetingId == new Guid(meetingData.MeetingId)).ExecuteUpdateAsync(

            b => b
            .SetProperty(prop => prop.MeetingDate, meetingData.MeetingDate)
            .SetProperty(prop => prop.MeetingTime, meetingData.MeetingTime)
            .SetProperty(prop => prop.Title, meetingData.Title)                       
            .SetProperty(prop => prop.ModifiedBy, userId)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        return true;
    }

    public async Task<bool> UpdatePostMeetingData(ConcludeMeetingRequestDTO meetingData, string userId) {
        var meeting = await dBContext.SMCMeeting.FirstOrDefaultAsync(x => x.MeetingId == new Guid(meetingData.MeetingId));
        if (meeting == null) return false;
        
        meeting.Status = (int)SMCMeetingStatus.CONCLUDED;
        meeting.Mom_Brief =  meetingData.MoM_Brief;
        meeting.Attendees = meetingData.Attendees.ToArray();
        meeting.ModifiedBy = userId;
        meeting.ModifiedDate = DateTime.UtcNow;
        
        return true;
    }

    public async Task<MeetingDetailsDTO?> GetMeetingDetails(Guid meetingId) {
        var meetingDetails = await (from a in dBContext.SMCMeeting
                                    join b in dBContext.CodeValues on a.Status equals b.CodeValue
                                    join c in dBContext.Branches on a.BranchId equals c.BranchId
                                    where a.MeetingId == meetingId
                                    select new MeetingDetailsDTO()
                                    {
                                        MeetingId = a.MeetingId,
                                        ForSession = a.ForSession,
                                        BranchId = a.BranchId,
                                        BranchName = c.BranchName,
                                        MeetingDate = a.MeetingDate,
                                        MeetingTime = a.MeetingTime,
                                        Mom_Brief = a.Mom_Brief,
                                        Status = a.Status,
                                        StatusDesc = b.CodeValDescription,
                                        Title = a.Title,                                        
                                        AgendaDetails = (from a in dBContext.SMCMeetingAgenda
                                                         join b in dBContext.CodeValues on a.AgendaCode equals b.CodeValue
                                                         where a.MeetingId == meetingId
                                                         select new MeetingAgendaDetailDTO()
                                                         {
                                                             MeetingId = a.MeetingId,
                                                             SerialNo = a.SerialNo,
                                                             AgendaCode = a.AgendaCode,
                                                             AgendaTitle = b.CodeValDescription,
                                                             OtherDetails = a.OtherDetails
                                                         }).ToList(),
                                        MeetingAttachments = dBContext.SMCMeetingAttachments.Where(x => x.MeetingId == meetingId)
                                                       .Select(x => new MeetingAttachmentsDTO()
                                                       {
                                                           MeetingId = x.MeetingId,
                                                           FileName = x.FileName,
                                                           FileURL = $"{x.FilePath}/{x.FileName}",
                                                           SerialNo = x.SerialNo,
                                                           Title = x.Title,
                                                           FileExtension = x.Extension
                                                       }).ToList(),
                                        InviteesDetails = a.Invitees != null ? (from x in dBContext.MemberRegistrations
                                                                                join mt in dBContext.CodeValues on x.MemberType equals mt.CodeValue
                                                                                join desig in dBContext.Designations on x.DesignationId equals desig.RowId
                                                                                join attd in a.Invitees on x.MemberId.ToString() equals attd
                                                                                //where a.Attendees.Contains(x.MemberId.ToString())//a.Attendees.Contains(attd => attd.Contains(x.MemberId.ToString()))
                                                                                select new InviteesDetailsDTO()
                                                                                {
                                                                                    MemberId = x.MemberId,
                                                                                    UniqueId = x.UniqueId,
                                                                                    DesignationId = x.DesignationId,
                                                                                    MemberName = x.Name,
                                                                                    MemberType = x.MemberType,
                                                                                    MobileNo = x.MobileNo,
                                                                                    MemberTypeDesc = mt.CodeValDescription,
                                                                                    DesignationTitle = desig.Title
                                                                                }).ToList() : null,
                                        AttendeesDetails = a.Attendees != null ? (from x in dBContext.MemberRegistrations
                                                                                  join mt in dBContext.CodeValues on x.MemberType equals mt.CodeValue
                                                                                  join desig in dBContext.Designations on x.DesignationId equals desig.RowId
                                                                                  join attd in a.Attendees on x.MemberId.ToString() equals attd
                                                                                  //where a.Attendees.Contains(x.MemberId.ToString())//a.Attendees.Contains(attd => attd.Contains(x.MemberId.ToString()))
                                                                                  select new AttendeesDetailsDTO()
                                                                                  {
                                                                                      MemberId = x.MemberId,
                                                                                      UniqueId = x.UniqueId,
                                                                                      DesignationId = x.DesignationId,
                                                                                      MemberName = x.Name,
                                                                                      MemberType = x.MemberType,
                                                                                      MobileNo = x.MobileNo,
                                                                                      MemberTypeDesc = mt.CodeValDescription,
                                                                                      DesignationTitle = desig.Title
                                                                                  }).ToList() : null,
                                        MeetingResolutions = dBContext.SMCMeetingResolutions.Where(r => r.MeetingId == a.MeetingId)
                                        .Select(resolution => new MeetingResolutionsDTO() { 
                                            AgendaSrNo = resolution.AgendaSrNo,
                                            ClosingDate = resolution.ClosingDate,
                                            Comments = resolution.Comments,
                                            EstimatedCost = resolution.EstimatedCost,
                                            IsClosed = resolution.IsClosed,
                                            Resolution = resolution.Resolution,
                                            ResolutionId = resolution.ResolutionId
                                        }).ToList()
                                    }).FirstOrDefaultAsync();
       

        return meetingDetails;
    }

    public async Task<List<MeetingsListDTO>?> GetMeetings(string branchId, string forSession) {
        var meetingDetails = await(from a in dBContext.SMCMeeting
                                   join b in dBContext.CodeValues on a.Status equals b.CodeValue
                                   join c in dBContext.Branches on a.BranchId equals c.BranchId
                                   where a.BranchId == branchId && a.ForSession == forSession
                                   select new MeetingsListDTO()
                                   {
                                       MeetingId = a.MeetingId,
                                       ForSession = a.ForSession,
                                       BranchId = a.BranchId,
                                       BranchName = c.BranchName,
                                       MeetingDate = a.MeetingDate,
                                       MeetingTime = a.MeetingTime,
                                       Mom_Brief = a.Mom_Brief,
                                       Status = a.Status,
                                       StatusDesc = b.CodeValDescription,
                                       Title = a.Title,
                                       TotalInvitees = a.Invitees != null ? a.Invitees.Length : 0,
                                       TotalAttendees = a.Attendees != null ? a.Attendees.Length : 0,
                                       TotalAgendas = (dBContext.SMCMeetingAgenda.Count(x=>x.MeetingId == a.MeetingId)),
                                       TotalResolutions = (dBContext.SMCMeetingResolutions.Count(x=>x.MeetingId == a.MeetingId))
                                   }).ToListAsync();
        return meetingDetails;
    }

    public async Task<List<MeetingsListDTO>?> GetMeetings(string branchId, string forSession, int status) {
        var meetingDetails = await(from a in dBContext.SMCMeeting
                                   join b in dBContext.CodeValues on a.Status equals b.CodeValue
                                   join c in dBContext.Branches on a.BranchId equals c.BranchId
                                   where a.BranchId == branchId && a.ForSession == forSession && a.Status == status
                                   select new MeetingsListDTO()
                                   {
                                       MeetingId = a.MeetingId,
                                       ForSession = a.ForSession,
                                       BranchId = a.BranchId,
                                       BranchName = c.BranchName,
                                       MeetingDate = a.MeetingDate,
                                       MeetingTime = a.MeetingTime,
                                       Mom_Brief = a.Mom_Brief,
                                       Status = a.Status,
                                       StatusDesc = b.CodeValDescription,
                                       Title = a.Title,
                                       TotalInvitees = a.Invitees != null ? a.Invitees.Length : 0,
                                       TotalAttendees = a.Attendees != null ? a.Attendees.Length : 0,
                                       TotalAgendas = (dBContext.SMCMeetingAgenda.Count(x => x.MeetingId == a.MeetingId)),
                                       TotalResolutions = (dBContext.SMCMeetingResolutions.Count(x => x.MeetingId == a.MeetingId))
                                   }).ToListAsync();
        return meetingDetails;
    }

    public async Task<bool> ValidateMeetingAgendaSerialNos(Guid meetingId, int[] agendaSrNos) {
        var agendaSrNoList = await dBContext.SMCMeetingAgenda.Where(x => x.MeetingId == meetingId).Select(x => x.SerialNo).ToArrayAsync();

        if (agendaSrNoList.Length == 0) return false;

        var invalidElements = agendaSrNos.Except(agendaSrNoList).ToArray();
        return invalidElements.Length == 0 ;
    }
}
