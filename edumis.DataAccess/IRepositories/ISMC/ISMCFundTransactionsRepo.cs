using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISMCFundTransactionsRepo : IRepository<SMCFundTransactionsModel>
{
    Task<SMCFundTransactionDetailDTO?> GetTransactionDetails(Guid TransactionId);
    Task<List<SmcFundTransactionShortDto>?> AllTransactions(string BranchId, DateOnly FromDate, DateOnly ToDate);
    Task Deactivate(Guid transactionId, string? remarks, string userId);
}
