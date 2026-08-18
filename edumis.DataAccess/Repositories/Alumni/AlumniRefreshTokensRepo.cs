using edumis.DataAccess.IRepositories.IAlumni;
using edumis.Models.Alumni.UserAccounts;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Alumni;

internal class AlumniRefreshTokensRepo(ApplicationDBContext dBContext) :
    Repository<AlumniRefreshTokenModel>(dBContext), IAlumniRefreshTokensRepo
{
    public async Task<AlumniRefreshTokenModel?> GetUserTokenDetails(string refreshToken)
    {
        return await dBContext.AlumniRefreshTokens
               .Include(x => x.User)
               .FirstOrDefaultAsync(x => x.Token == refreshToken);
    }

    public async Task RemoveUnusedTokensExcept(Guid UserId, string currentToken)
    {
        await dBContext.AlumniRefreshTokens.Where(x => x.Token != currentToken && x.UserId == UserId).ExecuteDeleteAsync();
    }
}
