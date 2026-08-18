using edumis.Models.Masters;
using edumis.Models.Masters.DTO;

namespace edumis.DataAccess.IRepositories.IMasters
{
    public interface IBranchRepo : IRepository<BranchesModel>
    {       
        Task<BranchDetailsDTO?> GetDetails(string BranchId);
        Task<List<BranchDetailsDTO>?> GetBranches();
        public Task<BranchesNamesDTO?> GetParentBranch(string branchId);
    }
}
