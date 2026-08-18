using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IEmployees;
using edumis.DataAccess.Mappers.Employees;
using edumis.Models;
using edumis.Models.Employees.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Employees;

internal class EmployeeEducationRepo : Repository<edumis.Models.Employees.EducationModel>, IEmployeeEducationRepo
{
    private readonly ApplicationDBContext dBContext;

    public EmployeeEducationRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    #region Employee Educational Details

    public async Task<bool> AddEducationalDetails(EducationDTO EducationModel, string CreatedBy, long? recordid)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType= NpgsqlDbType.Varchar, ParamValue = EducationModel.EmployeeId},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_serialno", DBType= NpgsqlDbType.Integer, ParamValue = EducationModel.SerialNo},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_qualification", DBType= NpgsqlDbType.Integer, ParamValue = EducationModel.Qualification },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_title", DBType= NpgsqlDbType.Varchar, ParamValue = EducationModel.Title},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_issuedate", DBType= NpgsqlDbType.Date, ParamValue = EducationModel.IssueDate},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_board", DBType= NpgsqlDbType.Varchar, ParamValue = EducationModel.Board},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_percentage", DBType= NpgsqlDbType.Numeric, ParamValue = EducationModel.Percentage},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_grade", DBType= NpgsqlDbType.Varchar, ParamValue = EducationModel.Grade},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_subjects", DBType= NpgsqlDbType.Varchar, ParamValue = EducationModel.Subjects},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = CreatedBy},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_recordid", DBType= NpgsqlDbType.Bigint, ParamValue = recordid == null ? 0: recordid}
            };

        try
        {
            ErrorModel? error = null;
            return await ExecStoredProcedureWithTrans(@"call spemp_educationdetailsins(
                                                                    :p_employeeid,
                                                                    :p_serialno,
                                                                    :p_qualification,
                                                                    :p_title,
                                                                    :p_issuedate,
                                                                    :p_board,
                                                                    :p_percentage,
                                                                    :p_grade,
                                                                    :p_subjects,
                                                                    :p_userid,
                                                                    :p_recordid)", spParamList, error);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<EducationDetailsDTO>?> GetEducationDetails(string EmployeeID)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_employeeid", DBType= NpgsqlDbType.Varchar, ParamValue = EmployeeID }
            };

        ErrorModel error = null;
        return await ExecuteSPReader("select * from spemp_geteducationdetails(:p_employeeid)", spParamList, EmployeeMapper.ToEducationalDetails, error);
    }

    #endregion
}
