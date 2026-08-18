using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.ITenders;
using edumis.Models;
using edumis.Models.Tenders;
using edumis.Models.Tenders.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Tenders;

internal class TendersRepo : Repository<TendersModel>, ITendersRepo
{
    private readonly ApplicationDBContext dBContext;
    public TendersRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> UpdateTender(long recordId, TenderUpdateRequestDTO tendersModel, string fileName, string userID)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_id", DBType= NpgsqlDbType.Bigint, ParamValue = recordId},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_tenderdate", DBType= NpgsqlDbType.Date, ParamValue = tendersModel.TenderDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_title", DBType= NpgsqlDbType.Text, ParamValue = tendersModel.Title},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_description", DBType= NpgsqlDbType.Text, ParamValue = tendersModel.Description},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_filelink", DBType= NpgsqlDbType.Varchar, ParamValue = fileName},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_expirydate", DBType= NpgsqlDbType.Date, ParamValue = tendersModel.ExpiryDate},                
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_modifiedby", DBType= NpgsqlDbType.Varchar, ParamValue = userID}
            };



        ErrorModel? error = null;
        return await ExecStoredProcedureWithTrans(@"call spweb_tenderupdate(
                                                                    :p_id,
                                                                    :p_tenderdate,
                                                                    :p_title,
                                                                    :p_description,                                                                   
                                                                    :p_filelink,
                                                                    :p_expirydate,                                                                                                                                       
                                                                    :p_modifiedby)", spParamList, error);

    }
}
