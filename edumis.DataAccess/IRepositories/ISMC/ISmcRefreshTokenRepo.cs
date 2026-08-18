using edumis.Models.SMC;
using edumis.Models.Users;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISmcRefreshTokenRepo : IRepository<SmcRefreshTokenModel>
{
    Task<SmcRefreshTokenModel?> GetUserTokenDetails(string refreshToken);

    Task RemoveUnusedTokensExcept(Guid UserId, string? currentToken);
}
