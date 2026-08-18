using edumis.DataAccess.IRepositories.IUsers;
using edumis.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Users;

internal class RefreshTokenRepo : Repository<RefreshTokenModel>, IRefreshTokenRepo
{
    private readonly ApplicationDBContext dBContext;
    public RefreshTokenRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<RefreshTokenModel?> GetUserTokenDetails(string refreshToken)
    {
        return await dBContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);
    }

    public async Task RemoveUnusedTokensExcept(Guid UserId, string currentToken)
    {
        await dBContext.RefreshTokens.Where(x => x.Token != currentToken && x.UserId == UserId).ExecuteDeleteAsync();
    }
}
