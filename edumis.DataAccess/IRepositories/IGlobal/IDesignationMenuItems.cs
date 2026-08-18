using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal
{
    public interface IDesignationMenuItems : IRepository<DesignationMenuItems>
    {
        Task<bool> MapDesignationWithMenu(DesignationMenuItemsDTO designationMenuItem);
        Task<List<DesignationMenuItemsDetailsDTO>> GetAllMappings();
    }
}
