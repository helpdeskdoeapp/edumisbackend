using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models.Global;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Global;

internal class CodesRepo(ApplicationDBContext dBContext) : Repository<CodesModel>(dBContext), ICodesRepo
{
    public async Task<List<CodesModel>?> GetMasterCodeDetails()
    {
        return await dBContext.Codes
            .AsNoTracking()
            .Include(c => c.CodeValuesList).ToListAsync();
    }

    public async Task<CodesModel?> GetMasterCodeDetails(int code)
    {
        return await dBContext.Codes
            .Where(c => c.Code == code) 
            .AsNoTracking()
            .Include(c => c.CodeValuesList).FirstOrDefaultAsync();
    }

    public async Task<List<CodesModel>?> GetMasterCodeDetails(List<int> codesList)
    {
        return await dBContext.Codes
           .Where(x => codesList.Contains(x.Code))
          .AsNoTracking()
          .Include(c => c.CodeValuesList).ToListAsync();
    }
}
