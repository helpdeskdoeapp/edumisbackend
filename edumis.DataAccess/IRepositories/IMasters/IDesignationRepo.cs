using edumis.Models.Masters;
using edumis.Models.Masters.DTO;

namespace edumis.DataAccess.IRepositories.IMasters;

public interface IDesignationRepo : IRepository<DesignationModel>
{
    Task<List<DesignationDetailsDTO>?> GetDesignations();
    Task<bool> Update(DesignationUpdateRequestDTO requestDTO, string userId);
    Task<bool> UpdateStatus(int designationId, bool status, string userId);
}
