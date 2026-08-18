using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.ICirculars;
using edumis.Models;
using edumis.Models.Circulars.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Circulars;

internal class CircularRepo : Repository<Models.Circulars.CircularModel>, ICircular
{

    private readonly ApplicationDBContext dBContext;
    public CircularRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> Update(CircularUpdateRequestDTO circular, string fileName, string userId)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_recordid", DBType= NpgsqlDbType.Bigint, ParamValue = circular.RecordId},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_circulardate", DBType= NpgsqlDbType.Date, ParamValue = circular.CircularDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_title", DBType= NpgsqlDbType.Text, ParamValue = circular.Title},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_description", DBType= NpgsqlDbType.Text, ParamValue = string.IsNullOrEmpty(circular.Description) ? "" : circular.Description },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_type", DBType= NpgsqlDbType.Integer, ParamValue = circular.Type},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_filelink", DBType= NpgsqlDbType.Varchar, ParamValue = fileName},                                
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = userId}
            };

        ErrorModel? error = null;
        return await ExecStoredProcedureWithTrans(@"call spweb_updatecircular(
                                                                    :p_recordid,
                                                                    :p_circulardate,
                                                                    :p_title,
                                                                    :p_description,
                                                                    :p_type,
                                                                    :p_filelink,                                                                                                                                                                                                        
                                                                    :p_userid)", spParamList, error);

    }
}
