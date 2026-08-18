using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IGlobal;
using edumis.Models;
using edumis.Models.Global.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Global;

internal class SessionInfoRepo : Repository<Models.Global.SessionInfoModel>, ISessionInfo
{
    private readonly ApplicationDBContext dBContext;
    public SessionInfoRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<bool> Update(SessionInfoDTO sessiondetails)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_forsession", DBType= NpgsqlDbType.Varchar, ParamValue = sessiondetails.ForSession },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isvalid", DBType= NpgsqlDbType.Boolean, ParamValue = sessiondetails.IsValid },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_iscurrent", DBType= NpgsqlDbType.Boolean, ParamValue = sessiondetails.IsCurrent },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isregistrationopen", DBType= NpgsqlDbType.Boolean, ParamValue = sessiondetails.IsRegistrationOpen },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_registrationstartdate", DBType= NpgsqlDbType.Date, ParamValue = sessiondetails.RegistrationStartDate == null ? Convert.ToDateTime("01/01/1900"): sessiondetails.RegistrationStartDate },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_registrationenddate", DBType= NpgsqlDbType.Date, ParamValue = sessiondetails.RegistrationEndDate == null ? Convert.ToDateTime("01/01/1900"): sessiondetails.RegistrationEndDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_lateregistrationstartdate", DBType= NpgsqlDbType.Date, ParamValue = sessiondetails.LateRegistrationStartDate == null ? Convert.ToDateTime("01/01/1900") : sessiondetails.LateRegistrationStartDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_lateregistrationenddate", DBType= NpgsqlDbType.Date, ParamValue = sessiondetails.LateRegistrationEndDate == null ? Convert.ToDateTime("01/01/1900") : sessiondetails.LateRegistrationEndDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_registrationendtime", DBType= NpgsqlDbType.Time, ParamValue = sessiondetails.RegistrationEndTime == null ? TimeOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : sessiondetails.RegistrationEndTime},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_lateregistrationendtime", DBType= NpgsqlDbType.Time, ParamValue = sessiondetails.LateRegistrationEndTime == null ? TimeOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : sessiondetails.LateRegistrationEndTime},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_reg_ageasondate", DBType= NpgsqlDbType.Date, ParamValue = sessiondetails.Reg_AgeAsOnDate == null ? Convert.ToDateTime("01/01/1900") : sessiondetails.Reg_AgeAsOnDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_registrationstarttime", DBType= NpgsqlDbType.Time, ParamValue = sessiondetails.RegistrationStartTime == null ? TimeOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : sessiondetails.RegistrationStartTime},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_lateregistrationstarttime", DBType= NpgsqlDbType.Time, ParamValue = sessiondetails.LateRegistrationStartTime  == null ? TimeOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : sessiondetails.LateRegistrationStartTime},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = sessiondetails.LoggedInUserId}
            };

        try
        {
            ErrorModel error = null;
            return await ExecStoredProcedureWithTrans(@"call spgl_updateacademicdetails(
                                                                    :p_forsession,
                                                                    :p_isvalid,
                                                                    :p_iscurrent,
                                                                    :p_isregistrationopen,
                                                                    :p_registrationstartdate,
                                                                    :p_registrationenddate,
                                                                    :p_lateregistrationstartdate,
                                                                    :p_lateregistrationenddate,
                                                                    :p_registrationendtime,
                                                                    :p_lateregistrationendtime,
                                                                    :p_reg_ageasondate,
                                                                    :p_registrationstarttime,
                                                                    :p_lateregistrationstarttime,
                                                                    :p_userid)", spParamList, error);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
