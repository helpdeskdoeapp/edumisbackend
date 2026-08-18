using edumis.Models.Global;
using edumis.Models.Global.DTO;

namespace edumis.DataAccess.IRepositories.IGlobal;

public interface IMenusRepo : IRepository<MenusModel>
{
    Task<List<MenuDetailDTO>?> GetAllMenus();
    Task<bool> Update(MenusUpdateRequestDTO requestDTO, string userId);
    Task<int?> GetMaxMenuId();
}
