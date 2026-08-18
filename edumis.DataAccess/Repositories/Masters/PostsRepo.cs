using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IMasters;
using edumis.Models;
using edumis.Models.Masters.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Masters;

internal class PostsRepo : Repository<Models.Masters.PostsModel>, IPostsRepo
{
    private readonly ApplicationDBContext dBContext;
    public PostsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> Update(PostsDTO PostDetails)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_postcode", DBType= NpgsqlDbType.Varchar, ParamValue = PostDetails.PostCode },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_posttitle", DBType= NpgsqlDbType.Varchar, ParamValue = PostDetails.PostTitle },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isgazetted", DBType= NpgsqlDbType.Boolean, ParamValue = PostDetails.IsGazetted },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_orderno", DBType= NpgsqlDbType.Varchar, ParamValue = PostDetails.OrderNo },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_orderdate", DBType= NpgsqlDbType.Timestamp, ParamValue = PostDetails.OrderDate == null ? Convert.ToDateTime("01/01/1900"): PostDetails.OrderDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isvalid", DBType= NpgsqlDbType.Boolean, ParamValue = PostDetails.IsValid},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = PostDetails.LoggedInUserId}
            };

        try
        {
            ErrorModel error = null;
            return await ExecStoredProcedureWithTrans(@"call spms_postdetailupdate(
                                                                   :p_postcode,
                                                                   :p_posttitle,
                                                                   :p_isgazetted,
                                                                   :p_orderno,
                                                                   :p_orderdate,
                                                                   :p_isvalid,
                                                                   :p_userid)", spParamList, error);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
