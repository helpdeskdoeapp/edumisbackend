using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IGlobal;
using edumis.DataAccess.Mappers;
using edumis.Models;
using edumis.Models.Global.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Global;

internal class DesignationMenuItemsRepo : Repository<Models.Global.DesignationMenuItems>, IDesignationMenuItems
{
    private readonly ApplicationDBContext dBContext;
    public DesignationMenuItemsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<List<DesignationMenuItemsDetailsDTO>> GetAllMappings()
    {
        ErrorModel error = null;
        return await ExecuteSPReader("select * from spgl_alldesignationmenumappings()", null, DesignationMenuMapper.ToMappings, error);
    }

    public async Task<bool> MapDesignationWithMenu(DesignationMenuItemsDTO designationMenuItem)
    {
        var spParamList = new ParamHelper
        {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designationid", DBType= NpgsqlDbType.Integer, ParamValue = designationMenuItem.DesignationId },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_menuid", DBType= NpgsqlDbType.Integer, ParamValue = designationMenuItem.MenuId },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_canview", DBType= NpgsqlDbType.Boolean, ParamValue = designationMenuItem.CanView},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_cancreate", DBType= NpgsqlDbType.Boolean, ParamValue = designationMenuItem.CanCreate },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_canedit", DBType= NpgsqlDbType.Boolean, ParamValue = designationMenuItem.CanEdit},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_candelete", DBType= NpgsqlDbType.Boolean, ParamValue = designationMenuItem.CanDelete},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = designationMenuItem.UserId}
            };
        try
        {
            ErrorModel error = null;
            return await ExecStoredProcedureWithTrans(@"call spgl_mapdesignationwithmenu(                                                                  
                                                                    :p_designationid,
                                                                    :p_menuid,
                                                                    :p_canview,
                                                                    :p_cancreate,
                                                                    :p_canedit,
                                                                    :p_candelete,                                                       
                                                                    :p_userid)", spParamList, error);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
