using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IEmployees;
using edumis.DataAccess.Mappers.Employees;
using edumis.Models;
using edumis.Models.Employees;
using edumis.Models.Employees.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Employees;

internal class EmployeeAppointmentRepo : Repository<AppointmentModel>, IEmployeeAppointmentRepo
{
    private readonly ApplicationDBContext dBContext;

    public EmployeeAppointmentRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }
    
    public async Task<AppointmentDetailsDTO?> GetAppointmentDetails(string EmployeeID)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType= NpgsqlDbType.Varchar, ParamValue = EmployeeID }
            };

        ErrorModel error = null;
        var ReturnDetails = await ExecuteSPReader("select * from spemp_getappointmentdetails(:p_employeeid)", spParamList, EmployeeMapper.ToAppointmentDetails, error);
        return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    }

    public async Task<bool> UpdateAppointmentDetails(AppointmentDTO AppointmentModel, string UpdatedBy)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType= NpgsqlDbType.Varchar, ParamValue = AppointmentModel.EmployeeId},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designation", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.Designation},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_seniorityno", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.SeniorityNo },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_appointmenttype", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.AppointmentType},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_appointmentorder", DBType= NpgsqlDbType.Varchar, ParamValue = AppointmentModel.AppointmentOrder},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_branchjoiningdate", DBType= NpgsqlDbType.Date, ParamValue = AppointmentModel.BranchJoiningDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_recruitmenttype", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.RecruitmentType},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_selectioncategory", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.SelectionCategory },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_currentpostheld", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.CurrentPostHeld},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_currentbranchid", DBType= NpgsqlDbType.Varchar, ParamValue = AppointmentModel.CurrentBranchID},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_cadre", DBType= NpgsqlDbType.Integer, ParamValue = AppointmentModel.Cadre},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_currentscale", DBType= NpgsqlDbType.Varchar, ParamValue = AppointmentModel.CurrentScale},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_grade", DBType= NpgsqlDbType.Varchar, ParamValue = AppointmentModel.Grade},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_appointmentdate", DBType= NpgsqlDbType.Date, ParamValue = AppointmentModel.AppointmentDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_gradegrantdate", DBType= NpgsqlDbType.Date, ParamValue = AppointmentModel.GradeGrantDate == null ? DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")):AppointmentModel.GradeGrantDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_retirementdate", DBType= NpgsqlDbType.Date, ParamValue = AppointmentModel.RetirementDate == null ?DateOnly.FromDateTime(Convert.ToDateTime("01/01/1900")) : AppointmentModel.RetirementDate },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = UpdatedBy}
            };

        try
        {
            ErrorModel? error = null;
            return await ExecStoredProcedureWithTrans(@"call spemp_appointmentdetailsinsupd(
                                                                    :p_employeeid,
                                                                    :p_designation,
                                                                    :p_seniorityno,
                                                                    :p_appointmenttype,
                                                                    :p_appointmentorder,                                                                    
                                                                    :p_branchjoiningdate,
                                                                    :p_recruitmenttype,
                                                                    :p_selectioncategory,
                                                                    :p_currentpostheld,
                                                                    :p_currentbranchid,
                                                                    :p_cadre,
                                                                    :p_currentscale,
                                                                    :p_grade,
                                                                    :p_appointmentdate,
                                                                    :p_gradegrantdate,
                                                                    :p_retirementdate,
                                                                    :p_userid)", spParamList, error);

        }
        catch
        {
            return false;
        }
    }  
}
