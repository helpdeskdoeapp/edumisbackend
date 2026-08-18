using edumis.Models.Users;
using edumis.Models.Users.DTO;

namespace edumis.DataAccess.IRepositories.IUsers
{
    public interface IUserRepo : IRepository<UserModel>
    {
        Task<UserDTO?> GetUserDetails(Guid UserId);
        Task<UserDTO?> GetMasterAdminUserDetails(Guid UserId);
        Task<UserDTO?> GetBranchAdminUserDetails(Guid UserId);
        Task<UserDTO?> GetBranchUserDetails(Guid UserId);
        Task<UserDetailsDTO> ValidateUser(string UserName);
        Task<bool> UpdatePassword(string UserID, string PasswordHash);
        Task<object?> GetUserProfile(Guid userId);
    }
}
