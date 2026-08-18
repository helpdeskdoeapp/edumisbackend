using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.SMC;

internal class MeetingResolutionsRepo : Repository<MeetingResolutionsModel>, IMeetingResolutionsRepo
{
    private readonly ApplicationDBContext dBContext;
    public MeetingResolutionsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> CloseResolution(CloseMeetingResolutionRequestDTO requestDto, string userId) {
        var resolution = await dBContext.SMCMeetingResolutions
            .FirstOrDefaultAsync(x => x.ResolutionId == new Guid(requestDto.ResolutionId));
        if (resolution == null) return false;
        
        resolution.IsClosed = true;
        resolution.ClosingDate = requestDto.ClosingDate;
        resolution.Comments = requestDto.Comments;
        resolution.ModifiedBy = userId;
        resolution.ModifiedDate = DateTime.UtcNow;
        return true;
    }

    public async Task<List<MeetingResolutionDetailsDTO>?> GetResolutionList(string BranchId, DateOnly FromDate, DateOnly ToDate)
    {
        var AllResolutions = await (from a in dBContext.SMCMeetingResolutions
                                join b in dBContext.SMCMeeting on a.MeetingId equals b.MeetingId
                                where b.BranchId == BranchId &&
                                DateOnly.FromDateTime(a.CreatedDate ?? Convert.ToDateTime("01/01/1990")) >= FromDate &&
                                DateOnly.FromDateTime(a.CreatedDate ?? DateTime.Today.Date) <= ToDate
                                select new MeetingResolutionDetailsDTO()
                                {
                                    ResolutionId = a.ResolutionId,
                                    MeetingId = a.MeetingId,
                                    ClosingDate = a.ClosingDate,
                                    Comments = a.Comments,
                                    IsClosed = a.IsClosed,
                                    Resolution = a.Resolution,
                                    EstimatedCost = a.EstimatedCost,
                                    ActualCost = a.ActualCost,
                                    CreatedDate = a.CreatedDate
                                }).ToListAsync(); 
       
        return AllResolutions;
    }

    public async Task<MeetingResolutionDetailsDTO?> ResolutionDetails(Guid ResolutionId)
    {
        var ResolutionData = await dBContext.SMCMeetingResolutions
            .Where(x => x.ResolutionId == ResolutionId)
            .Select(x => new MeetingResolutionDetailsDTO()
            {
                ResolutionId = x.ResolutionId,
                MeetingId = x.MeetingId,
                ClosingDate = x.ClosingDate,
                Comments = x.Comments,
                IsClosed = x.IsClosed,
                Resolution = x.Resolution,
                EstimatedCost = x.EstimatedCost,
                ActualCost = x.ActualCost,
                CreatedDate = x.CreatedDate,
                AgendaList = (x.AgendaSrNo != null && x.AgendaSrNo.Count() > 0)
                ? (from a in dBContext.SMCMeetingAgenda
                   join b in dBContext.CodeValues on a.AgendaCode equals b.CodeValue
                   where a.MeetingId == x.MeetingId && x.AgendaSrNo.Contains(a.SerialNo)
                   select (a.AgendaCode == Constants.OTHER_AGENDA_CODE ? a.OtherDetails : b.CodeValDescription)).ToList()
                : null,
                Transactions = (from tr in dBContext.SMCFundTransactions
                                .Where(t => t.ResolutionId == x.ResolutionId && t.IsActive==true)
                                .AsNoTracking()
                                .Include(t => t.SMCTransactionAttachmentsList)
                                select new SmcFundTransactionShortDto()
                                {
                                    TransactionId = tr.TransactionId,
                                    Description = tr.Description,
                                    Amount = tr.Amount,
                                    MeetingId = tr.MeetingId,
                                    TransactionDate = tr.TransactionDate,
                                    IsActive = tr.IsActive
                                }).ToList()
            }).FirstOrDefaultAsync();        

        return ResolutionData;
    }

    public async Task UpdateResolutionActualCost(Guid resolutionId, decimal actualCost, string userId) {
        var resolution = await dBContext.SMCMeetingResolutions
            .FirstOrDefaultAsync(x => x.ResolutionId == resolutionId);

        if (resolution == null) return;

        resolution.ActualCost = (resolution.ActualCost ?? 0) + actualCost;
        resolution.ModifiedBy = userId;
        resolution.ModifiedDate = DateTime.UtcNow;
        
    }
}
