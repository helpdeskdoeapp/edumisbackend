using edumis.Models.SMC;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISmcBudgetRepo : IRepository<SmcBudgetAllocationModel> {
    public Task<bool> UpdateExpense(string session, string schoolId, decimal amount, string userId);
    
    public Task AddOrUpdateAllocation(string session, string schoolId, decimal amount, string userId);
}