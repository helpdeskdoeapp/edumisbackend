using edumis.Models.Alumni.UserAccounts;
using edumis.Models.Users.DTO;

namespace edumis.DataAccess.IRepositories.IAlumni;

public interface IAlumniLoginRepo : IRepository<AlumniLoginModel>
{
    Task<UserDTO?> GetUserDetails(Guid AlumniId);
}
