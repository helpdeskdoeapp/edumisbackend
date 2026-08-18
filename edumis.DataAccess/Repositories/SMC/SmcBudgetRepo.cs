using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.SMC;

internal class SmcBudgetRepo(ApplicationDBContext dbContext) : Repository<SmcBudgetAllocationModel>(dbContext), ISmcBudgetRepo {
    private readonly ApplicationDBContext dbContext = dbContext;
    
    public async Task<bool> UpdateExpense(string session, string schoolId, decimal amount, string userId) {
        var entry = await dbContext.SmcBudgetAllocations
            .FirstOrDefaultAsync(x => x.Session == session && x.SchoolId == schoolId);
        if (entry == null) return false;
        
        entry.Consumption += amount;
        entry.ModifiedBy = userId;
        entry.ModifiedDate = DateTime.UtcNow;
        
        return true;
    }


    public async Task AddAllocations(List<SmcBudgetNewAllocationDto> list, string currentSession, string userId) {

        var sessions = list.Select(i => i.Session??currentSession).ToHashSet();
        var allocations = dbContext.SmcBudgetAllocations.Where(a => sessions.Contains(a.Session));
        foreach (var item in list) {
            var entry = allocations.FirstOrDefault(a => a.SchoolId == item.SchoolId && a.Session == item.Session);
            if (entry == null)
                await dbContext.SmcBudgetAllocations.AddAsync(new SmcBudgetAllocationModel {
                    Session = item.Session ?? currentSession,
                    SchoolId = item.SchoolId,
                    Allocation =  item.Amount,
                    Consumption = 0,
                    ModifiedBy = userId,
                    ModifiedDate = DateTime.UtcNow
                });
            else {
                entry.Allocation += item.Amount;
                entry.ModifiedDate = DateTime.UtcNow;
                entry.ModifiedBy = userId;
            }

        }
        
    }     
    public async Task AddOrUpdateAllocation(string session, string schoolId, decimal amount, string userId) {
        var entry = await dbContext.SmcBudgetAllocations
            .FirstOrDefaultAsync(x => x.Session == session && x.SchoolId == schoolId);
        if (entry == null) {
            await dbContext.SmcBudgetAllocations
                .AddAsync(new SmcBudgetAllocationModel {
                        Session = session,
                        SchoolId = schoolId,
                        Allocation = amount,
                        Consumption = 0
                    }
                );
            return;
        }
        
        entry.Allocation += amount;
        entry.ModifiedDate = DateTime.UtcNow;
        entry.ModifiedBy = userId;
       
    }
    
}