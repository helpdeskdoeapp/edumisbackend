using edumis.Models.Users;

namespace edumis.DataAccess.IRepositories.IUsers;

public interface IRefreshTokenRepo : IRepository<RefreshTokenModel>
{
    Task<RefreshTokenModel?> GetUserTokenDetails(string refreshToken);

    Task RemoveUnusedTokensExcept(Guid UserId, string currentToken);
}
