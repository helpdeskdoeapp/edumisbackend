using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISmcBudgetHistoryRepo : IRepository<SmcBudgetAllocationHistoryModel> {
    Task<List<SmcBudgetHistoryEntry>> GetAllocationHistory(string session, string schoolId);
}