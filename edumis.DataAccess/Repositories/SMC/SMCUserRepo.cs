using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.ISMC;
using edumis.DataAccess.Mappers.SMC;
using edumis.Models;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.SMC;

internal class SMCUserRepo : Repository<SMCAccountsModel>, ISMCUserRepo
{
    private readonly ApplicationDBContext dBContext;
    public SMCUserRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<SMCBranchDetailsDTO?> GetBranchUserDetails(string BranchUserId)
    {
        var BranchDetails = await(from a in dBContext.SMCAccounts
                            join b in dBContext.Branches on a.BranchId equals b.BranchId
                            join c in dBContext.CodeValues on a.UserType equals c.CodeValue
                            where a.UserId == new Guid(BranchUserId)
                            select new SMCBranchDetailsDTO()
                            {
                                UserId = a.UserId,
                                BranchId = a.BranchId,
                                BranchName = b.BranchName,
                                BranchType = b.BranchType,
                                IsValid = a.IsValid ?? false,
                                UserType = a.UserType,
                                UserTypeDesc = c.CodeValDescription
                            }).FirstOrDefaultAsync();
        return BranchDetails;
    }

    //public async Task<SMCUserDTO> GetUserDetails(string uniqueid, int type)
    //{
    //    var spParamList = new ParamHelper
    //        {
    //            new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_uniqueid", DBType= NpgsqlDbType.Varchar, ParamValue = uniqueid },
    //            new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_type", DBType= NpgsqlDbType.Integer, ParamValue = type }
    //        };

    //    ErrorModel error = null;
    //    var ReturnDetails = await ExecuteSPReader("select * from spsmc_getuserdetails(:p_uniqueid, :p_type)", spParamList, SMCUserMapper.ToUserDetails, error);
    //    return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    //}
}
