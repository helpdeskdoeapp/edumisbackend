using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models.Global.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace edumis.DataAccess.Repositories.Global;

internal class MenusRepo : Repository<Models.Global.MenusModel>, IMenusRepo
{
    private readonly ApplicationDBContext dBContext;
    public MenusRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> Update(MenusUpdateRequestDTO requestDTO, string userId)
    {
        var rowsAffected = await dBContext.Menus.Where(x => x.MenuId == requestDTO.MenuId).ExecuteUpdateAsync(b => b
            .SetProperty(prop => prop.MenuTitle, requestDTO.MenuTitle)
            .SetProperty(prop => prop.ParentMenuId, requestDTO.ParentMenuId == null ? 0 : requestDTO.ParentMenuId)
            .SetProperty(prop => prop.Module, requestDTO.Module == null ? 0 : requestDTO.Module)
            .SetProperty(prop => prop.Menuurl, requestDTO.Menuurl)
            .SetProperty(prop => prop.IsValid, requestDTO.IsValid)
            .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            .SetProperty(prop => prop.ModifiedBy, userId)
        );

        return rowsAffected > 0;

        #region Commented
        //var spParamList = new ParamHelper
        //{
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_menuid", DBType= NpgsqlDbType.Integer, ParamValue = menuDTO.MenuId },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_menutitle", DBType= NpgsqlDbType.Varchar, ParamValue = menuDTO.MenuTitle },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_parentmenuid", DBType= NpgsqlDbType.Integer, ParamValue = menuDTO.ParentMenuId == null ? 0 : menuDTO.ParentMenuId},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_module", DBType= NpgsqlDbType.Integer, ParamValue = menuDTO.Module == null ? 0 : menuDTO.Module },
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_menuurl", DBType= NpgsqlDbType.Varchar, ParamValue = menuDTO.Menuurl},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isvalid", DBType= NpgsqlDbType.Boolean, ParamValue = menuDTO.IsValid},
        //        new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = menuDTO.UserId}
        //    };
        //try
        //{
        //    ErrorModel error = null;
        //    return await ExecStoredProcedureWithTrans(@"call spgl_menuupdate(
        //                                                           :p_menuid,
        //                                                           :p_menutitle,
        //                                                           :p_parentmenuid,
        //                                                           :p_module,
        //                                                           :p_menuurl,
        //                                                           :p_isvalid,
        //                                                           :p_userid
        //                                                           )", spParamList, error);
        //}
        //catch (Exception ex)
        //{
        //    //return false;
        //    throw ex;
        //}
        #endregion
    }

    public async Task<List<MenuDetailDTO>?> GetAllMenus()
    {
        return await(from a in dBContext.Menus
                    join c in dBContext.CodeValues on a.Module equals c.CodeValue into cGroup
                    from c in cGroup.DefaultIfEmpty()
                    join b in dBContext.Menus on a.ParentMenuId equals b.MenuId into bGroup
                    from b in bGroup.DefaultIfEmpty()
                    orderby a.MenuTitle
                    select new MenuDetailDTO()
                    {
                        MenuId = a.MenuId,
                        MenuTitle = a.MenuTitle,
                        ParentMenuId = a.ParentMenuId,
                        ParentMenuTitle = b != null ? b.MenuTitle : null,
                        Module = a.Module,
                        ModuleTitle = c != null ? c.CodeValDescription : null,
                        Menuurl = a.Menuurl,
                        IsValid = a.IsValid                        
                    }).ToListAsync();

        // Uncomment the following lines if you want to use stored procedure instead of LINQ query
        //ErrorModel error = null;
        //return await ExecuteSPReader("select * from spgl_getallmenus()", null, MenuItemsMapper.ToMenuItems, error);
    }

    public async Task<int?> GetMaxMenuId()
    {
        return await dBContext.Menus          
           .OrderByDescending(x => x.MenuId)
           .Select(x => x.MenuId)
           .FirstOrDefaultAsync();
    }
}
