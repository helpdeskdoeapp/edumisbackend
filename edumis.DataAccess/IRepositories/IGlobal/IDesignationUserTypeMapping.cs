using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal
{
    public interface IDesignationUserTypeMapping : IRepository<DesignationUserTypeMapping>
    {
        Task<List<DesignationUserTypeMappingDetailsDTO>> GetAllMappings();
    }
}
