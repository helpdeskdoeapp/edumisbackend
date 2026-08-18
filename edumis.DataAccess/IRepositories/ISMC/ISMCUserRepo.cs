using edumis.Models.SMC;
using edumis.Models.SMC.DTO;

namespace edumis.DataAccess.IRepositories.ISMC;

public interface ISMCUserRepo : IRepository<SMCAccountsModel>
{
    Task<SMCBranchDetailsDTO?> GetBranchUserDetails(string BranchUserId);
}
