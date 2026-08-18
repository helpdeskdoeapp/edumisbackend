using edumis.Common;
using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.SMC;

internal class SMCFundTransactionsRepo : Repository<SMCFundTransactionsModel>, ISMCFundTransactionsRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMCFundTransactionsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<SmcFundTransactionShortDto>?> AllTransactions(string BranchId, DateOnly FromDate, DateOnly ToDate)
    {
       return await (
                        from a in dBContext.SMCFundTransactions
                        join b in dBContext.SMCMeeting on a.MeetingId equals b.MeetingId
                        where b.BranchId == BranchId &&
                            a.TransactionDate >= FromDate &&
                            a.TransactionDate <= ToDate
                        select new SmcFundTransactionShortDto()
                        {
                            TransactionId = a.TransactionId,
                            Description = a.Description,
                            Amount = a.Amount,
                            MeetingId = a.MeetingId,
                            TransactionDate = a.TransactionDate,
                            IsActive = a.IsActive,
                        }).ToListAsync();
    }

    public async Task Deactivate(Guid transactionId, string? remarks, string userId) {
        await dBContext.SMCFundTransactions.Where(x => x.TransactionId == transactionId).ExecuteUpdateAsync(
            b=>b
                .SetProperty(prop => prop.Remarks, remarks.IsNullOrBlank()?null:remarks )
                .SetProperty(prop => prop.IsActive, false)
                .SetProperty(prop => prop.ModifiedBy, userId)
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
        );
        
    }

    public async Task<SMCFundTransactionDetailDTO?> GetTransactionDetails(Guid TransactionId)
    {
        var transData = await dBContext.SMCFundTransactions
            .Where(x => x.TransactionId == TransactionId)
            .AsNoTracking()           
            .Include(x=>x.SMCTransactionAttachmentsList).ToListAsync();
        return (from a in transData
                join c in dBContext.CodeValues on a.TransactionMode equals c.CodeValue
                select new SMCFundTransactionDetailDTO()
                {
                    TransactionId = a.TransactionId,
                    Description = a.Description,
                    Amount = a.Amount,
                    MeetingId = a.MeetingId,
                    ReferenceDocNo = a.ReferenceDocNo,
                    ResolutionId = a.ResolutionId,
                    TransactionDate = a.TransactionDate,
                    TransactionMode = a.TransactionMode,
                    TransactionModeDesc = c.CodeValDescription,
                    IsActive = a.IsActive,
                    Remarks = a.Remarks,
                    LastModifiedDate = a.ModifiedDate switch{ null => null, _=> DateOnly.FromDateTime(a.ModifiedDate.Value)},
                    SMCTransactionAttachmentsList = a.SMCTransactionAttachmentsList.Select(x => new SMCTransactionAttachmentListDTO()
                    {
                        SerialNo = x.SerialNo,
                        ContentType = x.ContentType,
                        Extension = x.Extension,
                        FileName = x.FileName,
                        FilePath = x.FilePath,
                        Title = x.Title                        
                    }).ToList()
                    
                }).FirstOrDefault();
    }
}
