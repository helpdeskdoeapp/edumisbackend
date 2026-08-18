using edumis.DataAccess.IRepositories.ISMC;
using edumis.Models.SMC;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.SMC;

internal class SmcRefreshTokenRepo(ApplicationDBContext dBContext)
    : Repository<SmcRefreshTokenModel>(dBContext), ISmcRefreshTokenRepo {
    
    private readonly ApplicationDBContext dBContext = dBContext;

    public async Task<SmcRefreshTokenModel?> GetUserTokenDetails(string refreshToken)
    {
        return await dBContext.SmcRefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);
    }

    public async Task RemoveUnusedTokensExcept(Guid UserId, string? currentToken)
    {
        await dBContext.RefreshTokens.Where(x => x.Token != currentToken && x.UserId == UserId).ExecuteDeleteAsync();
    }
}
