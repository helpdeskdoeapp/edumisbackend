using edumis.Models.Alumni.UserAccounts;

namespace edumis.DataAccess.IRepositories.IAlumni;

public interface IAlumniRefreshTokensRepo : IRepository<AlumniRefreshTokenModel>
{
    Task<AlumniRefreshTokenModel?> GetUserTokenDetails(string refreshToken);

    Task RemoveUnusedTokensExcept(Guid UserId, string currentToken);
}
