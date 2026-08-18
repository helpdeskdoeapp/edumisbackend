using edumis.DataAccess.IRepositories.IAlumni;
using edumis.Models.Alumni.UserAccounts;
using edumis.Models.Users.DTO;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess.Repositories.Alumni;

internal class AlumniLoginRepo(ApplicationDBContext dBContext) : Repository<AlumniLoginModel>(dBContext), IAlumniLoginRepo
{
    public async Task<UserDTO?> GetUserDetails(Guid AlumniId)
    {
        return await (from a in dBContext.AlumniDetails
                      join l in dBContext.AlumniLogins on a.AlumniId equals l.AlumniID
                      join b in dBContext.Branches on a.BranchId equals b.BranchId into branches
                      from br in branches.DefaultIfEmpty()
                      where (a.AlumniId == AlumniId)
                      select new UserDTO
                      {
                          UserId = a.AlumniId,
                          UserName = $"{a.FirstName} {a.MiddleName} {a.LastName}",
                          UserType = 0,
                          UniqueId = a.RowId.ToString(),
                          BranchId = a.BranchId,
                          BranchTitle = br != null ? br.BranchName : string.Empty,
                          IsValid = l.IsValid,
                          IsAccountLocked = l.IsAccountLocked,
                          IsLoggedIn = l.IsLoggedIn                         
                      }).FirstOrDefaultAsync();
    }
}
