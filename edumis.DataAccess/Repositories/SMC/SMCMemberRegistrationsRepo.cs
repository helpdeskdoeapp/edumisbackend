using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.ISMC;
using edumis.DataAccess.Mappers.SMC;
using edumis.Models;
using edumis.Models.Employees.DTO;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.SMC;

internal class SMCMemberRegistrationsRepo : Repository<MemberRegistrationsModel>, ISMCMemberRegistrationsRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMCMemberRegistrationsRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<string> CreateMember(SMCMemberRequestDTO memberDetails, string BranchId)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_name", DBType= NpgsqlDbType.Varchar, ParamValue = memberDetails.Name },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designationid", DBType= NpgsqlDbType.Integer, ParamValue = memberDetails.DesignationId },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_membertype", DBType= NpgsqlDbType.Integer, ParamValue = memberDetails.MemberType },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_gender", DBType= NpgsqlDbType.Integer, ParamValue = memberDetails.Gender },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_mobileno", DBType= NpgsqlDbType.Varchar, ParamValue = memberDetails.MobileNo },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_branchid", DBType= NpgsqlDbType.Varchar, ParamValue = BranchId },
                //new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isactive", DBType= NpgsqlDbType.Boolean, ParamValue = memberDetails.IsActive },
               // new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = memberDetails.LoggedInUserId}
            };

        ErrorModel? error = null;
        object? ReturnVal = await ExecNonQueryTransSingle(@"select * from spsmc_createmember(                                                                    
                                                                    :p_name,
                                                                    :p_designationid,
                                                                    :p_membertype,
                                                                    :p_gender,
                                                                    :p_mobileno,
                                                                    :p_branchid,
                                                                    :p_isactive,
                                                                    :p_userid)", spParamList, error);

        return ReturnVal != null ? ReturnVal.ToString() : string.Empty;

    }

    public async Task<bool> UpdateMember(SMCMemberUpdateRequestDTO memberDetails, string BranchId, string UserId)
    {
        var rowsAffected = await dBContext.MemberRegistrations.Where(x => x.MemberId == new Guid(memberDetails.MemberId)
            && x.BranchId == BranchId).ExecuteUpdateAsync(
            b => b
                .SetProperty(prop => prop.Name, memberDetails.Name)
                .SetProperty(prop => prop.UniqueId, memberDetails.UniqueId)
                .SetProperty(prop => prop.DesignationId, memberDetails.DesignationId)
                .SetProperty(prop => prop.MemberType, memberDetails.MemberType)
                .SetProperty(prop => prop.Gender, memberDetails.Gender)
                .SetProperty(prop => prop.MobileNo, memberDetails.MobileNo)
                .SetProperty(prop => prop.IsActive, memberDetails.IsActive)
                .SetProperty(prop => prop.ModifiedBy, UserId)
                .SetProperty(prop => prop.ModifiedDate, DateTime.UtcNow)
            );
        
        //if(rowsAffected > 0)
        //{
        //    var userRowsAffected = await dBContext.SMCAccounts.Where(x => x.UniqueId == memberDetails.MemberId &&
        //       x.BranchId == memberDetails.BranchId).ExecuteUpdateAsync(
        //            b=>b.SetProperty(prop=>prop.IsValid, memberDetails.IsActive)
        //            .SetProperty(prop => prop.UserType, memberDetails.MemberType)
        //        );
        //}

        return rowsAffected == 0 ? false : true;
    }

    public async Task<SMCMemberDetailsDTO?> GetMemberDetails(string MemberID)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_memberid", DBType= NpgsqlDbType.Uuid, ParamValue = new Guid(MemberID) }
            };

        ErrorModel error = null;
        var ReturnDetails = await ExecuteSPReader("select * from spsmc_getemployeedetails(:p_memberid)", spParamList, SMCEmployeeMapper.ToEmployeeDetails, error);
        return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    }


    public async Task<List<SMCMemberDetailsDTO>> SearchMembers(SearchSMCTeamMembers searchEmployee) 
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_forsession", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.ForSession ?? string.Empty },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_designationid", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.DesignationId ?? 0 },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_membertype", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.MemberType ?? 0 },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_branchid", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.BranchId ?? string.Empty},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_district", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.District ?? string.Empty},
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_zone", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.Zone?? string.Empty },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_gender", DBType= NpgsqlDbType.Integer, ParamValue = searchEmployee.Gender ?? 0 },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_mobileno", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.MobileNo?? string.Empty },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isactive", DBType= NpgsqlDbType.Varchar, ParamValue = searchEmployee.Status switch { true => "Y", false => "N", _ => "ALL"} }
            };

        ErrorModel error = null;
        return await ExecuteSPReader(@"select * from spsmc_searchmembers(
                                                        :p_forsession,
                                                        :p_designationid,
                                                        :p_membertype,
                                                        :p_branchid,
                                                        :p_district,
                                                        :p_zone,
                                                        :p_gender,
                                                        :p_mobileno,
                                                        :p_isactive)", spParamList, SMCEmployeeMapper.ToSearchResult, error);

    }

    public async Task<bool> UpdateStatus(string MemberId, bool Status, string UserId)
    {
        await dBContext.MemberRegistrations.Where(x => x.MemberId == new Guid(MemberId)).ExecuteUpdateAsync(
            b=>b
            .SetProperty(prop=>prop.IsActive, Status)
            .SetProperty(prop=>prop.ModifiedBy, UserId)
            .SetProperty(prop=>prop.ModifiedDate,DateTime.Now)
            );

        //await dBContext.SMCAccounts.Where(x => x.UniqueId == memberData.MemberId).ExecuteUpdateAsync(
        //     b => b
        //    .SetProperty(prop => prop.IsValid, false)
        //    .SetProperty(prop => prop.ModifiedBy, memberData.LoggedInUserId)
        //    .SetProperty(prop => prop.ModifiedDate, DateTime.Now)
        //    );

        return true;
    }

    public async Task<List<SMCMemberDetailsDTO>> GetAllMembers(string MobileNo)
    {
        var spParamList = new ParamHelper
        {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_mobileno", DBType= NpgsqlDbType.Varchar, ParamValue = MobileNo }                
        };

        ErrorModel error = null;
        return await ExecuteSPReader(@"select * from spsmc_getmembersbymobileno(
                                                        :p_mobileno)", spParamList, SMCEmployeeMapper.ToSearchResult, error);
    }
}
