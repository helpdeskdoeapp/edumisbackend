using edumis.Common;
using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.SMC;

internal class SmcBudgetHistoryRepo(ApplicationDBContext dbContext)
    : Repository<SmcBudgetAllocationHistoryModel>(dbContext), ISmcBudgetHistoryRepo {
    public async Task<List<SmcBudgetHistoryEntry>> GetAllocationHistory(string session, string schoolId) {
        return await (
            from a in dbContext.SmcBudgetAllocationHistory
            join b in dbContext.CodeValues on a.AllocationType equals b.CodeValue
            where a.Session == session &&
                  a.SchoolId == schoolId
            select new SmcBudgetHistoryEntry{
                Session = a.Session,
                SchoolId = a.SchoolId,
                Amount = a.Amount,
                AllocationType = a.AllocationType,
                AllocationTypeDesc = b.CodeValDescription,
                AllocationDate = a.AllocationDate,
                DonorName = a.DonorName,
                DonorPan = a.DonorPan,
                DonorMobile = a.DonorMobile,
                DonorAddress = a.DonorAddress,
                Remarks = a.Remarks
            }).ToListAsync();
    }
}