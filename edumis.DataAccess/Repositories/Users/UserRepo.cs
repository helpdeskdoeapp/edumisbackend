using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories;
using edumis.DataAccess.IRepositories.IUsers;
using edumis.DataAccess.Mappers;
using edumis.Models;
using edumis.Models.Users.DTO;
using edumis.Models.Users.DTO.UserProfile;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Users;

internal class UserRepo : Repository<Models.Users.UserModel>, IUserRepo
{
    private readonly ApplicationDBContext dBContext;
    public UserRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<UserDetailsDTO> ValidateUser(string UserName)
    {
        var spParamList = new ParamHelper
                {
                    new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_username", DBType= NpgsqlDbType.Varchar, ParamValue = UserName }
                };

        ErrorModel error = null;
        var ReturnDetails = await ExecuteSPReader(@"select * from spms_getuser(:p_username)", spParamList, UserMapper.ToUser, error);
        return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    }

    public async Task<UserDTO?> GetUserDetails(Guid UserId)
    {
        var spParamList = new ParamHelper
                {
                    new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "puserid", DBType= NpgsqlDbType.Uuid, ParamValue = UserId }
                };

        ErrorModel error = null;
        var ReturnDetails = await ExecuteSPReader(@"select * from spms_getuserdetails(:puserid)", spParamList, UserMapper.ToUserDetails, error);
        return ReturnDetails.Any() ? ReturnDetails.FirstOrDefault() : null;
    }

    public async Task<bool> UpdatePassword(string UserID, string PasswordHash)
    {
        var spParamList = new ParamHelper
                {
                    new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "puserid", DBType= NpgsqlDbType.Uuid, ParamValue = new Guid(UserID) },
                    new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "ppassword", DBType= NpgsqlDbType.Varchar, ParamValue = PasswordHash }
                };
        ErrorModel error = null;
        return await ExecStoredProcedureWithTrans(@"call spms_updatepassword(:puserid, :ppassword)", spParamList, error);
    }

    public async Task<UserDTO?> GetMasterAdminUserDetails(Guid UserId)
    {
        return await (from a in dBContext.Users
                      join b in dBContext.CodeValues on a.UserType equals b.CodeValue
                      where a.UserId == UserId
                      select new UserDTO()
                      {                          
                          UniqueId = a.UniqueId,
                          BranchTitle = b.CodeValDescription,
                          IsAccountLocked = a.IsAccountLocked,
                          Designation = b.CodeValDescription,
                          IsLoggedIn = a.IsLoggedIn,
                          IsValid = a.IsValid,
                          UserId = a.UserId,
                          UserName = b.CodeValDescription,
                          UserType = a.UserType,
                          UserRole = a.UserRole
                      }).FirstOrDefaultAsync();
    }

    public async Task<UserDTO?> GetBranchAdminUserDetails(Guid UserId)
    {
        return await (from a in dBContext.Users
                      join b in dBContext.Branches on a.UniqueId equals b.BranchId
                      where a.UserId == UserId
                      select new UserDTO()
                      {
                          BranchId = b.BranchId,
                          UniqueId = a.UniqueId,
                          BranchTitle = b.BranchName,
                          IsAccountLocked = a.IsAccountLocked,
                          IsLoggedIn = a.IsLoggedIn,
                          IsValid = a.IsValid,
                          UserId = a.UserId,
                          UserName = b.BranchName,
                          UserType = a.UserType,
                          UserRole = a.UserRole
                      }).FirstOrDefaultAsync();
    }

    public async Task<UserDTO?> GetBranchUserDetails(Guid UserId)
    {
        return await(from a in dBContext.Users
                     join b in dBContext.Employees on a.UniqueId equals b.EmployeeId
                     join c in dBContext.EmployeeAppointmentDetails on b.EmployeeId equals c.EmployeeId
                     join d in dBContext.Branches on c.CurrentBranch equals d.BranchId
                     join e in dBContext.Designations on c.Designation equals e.RowId
                     join f in dBContext.CodeValues on e.DesignationGroup equals f.CodeValue
                     where a.UserId == UserId
                     select new UserDTO()
                     {
                         BranchId = d.BranchId,
                         UniqueId = a.UniqueId,
                         BranchTitle = d.BranchName,
                         IsAccountLocked = a.IsAccountLocked,
                         IsLoggedIn = a.IsLoggedIn,
                         IsValid = a.IsValid,
                         UserId = a.UserId,
                         UserName = $"{b.FirstName} {b.MiddleName} {b.LastName}",
                         UserType = a.UserType,
                         UserRole = a.UserRole,
                         DesignationId = c.Designation,
                         Designation = e.Title,
                         DesignationGroupId = e.DesignationGroup,
                         DesignationGroup = f.CodeValDescription
                     }).FirstOrDefaultAsync();
    }

    public async Task<object?> GetUserProfile(Guid userId)
    {
        var userData = await dBContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        if (userData == null)
            return null;

        switch (userData.UserType)
        {
            case 201: //Master Admin
                return await (from a in dBContext.Users
                              join b in dBContext.CodeValues on a.UserType equals b.CodeValue
                              where a.UserId == userId
                              select new MasterAdminUserProfileResponseDTO
                              {
                                  UniqueId = a.UniqueId,
                                  UserId = a.UserId,
                                  FullName = b.CodeValDescription,
                                  ProfileImageContentType = a.Photo,
                                  EmailId = a.EmailId                               
                              }).FirstOrDefaultAsync();              
            case 202://Branch Admin                
            case 203:
            case 204:
                return await (from a in dBContext.Users
                              join b in dBContext.Branches on a.UniqueId equals b.BranchId
                              join c in dBContext.Districts on b.DistrictId equals c.RowId into districtGroup
                              from district in districtGroup.DefaultIfEmpty()
                              join d in dBContext.Zones on b.ZoneId equals d.RowId into zoneGroup
                              from zone in zoneGroup.DefaultIfEmpty()
                              join e in dBContext.Employees on a.UniqueId equals e.EmployeeId into employeeGroup
                              from incharge in employeeGroup.DefaultIfEmpty()
                              where a.UserId == userId
                              select new BranchAdminUserProfileResponseDTO()
                              {
                                  BranchId = b.BranchId,
                                  UniqueId = a.UniqueId,
                                  UserId = a.UserId,
                                  FullName = b.BranchName,
                                  EmailId = a.EmailId,
                                  MobileNo = b.ContactNo,
                                  ContactNumber = b.ContactNo,
                                  ProfileImageUrl = a.Photo,
                                  Address = b.Address,
                                  District = district.Title,
                                  Zone = zone.Title,
                                  BranchName = b.BranchName,
                                  InchargeId = b.InchargeId,
                                  InchargeName = incharge != null ? $"{incharge.FirstName} {incharge.MiddleName} {incharge.LastName}"
                                        : string.Empty
                              }).FirstOrDefaultAsync();            
            case 205://Branch Employee User
            case 206:
                return await (from a in dBContext.Users
                              join b in dBContext.Employees on a.UniqueId equals b.EmployeeId
                              join c in dBContext.EmployeeAppointmentDetails on b.EmployeeId equals c.EmployeeId
                              join d in dBContext.Branches on c.CurrentBranch equals d.BranchId
                              join e in dBContext.Designations on c.Designation equals e.RowId
                              join f in dBContext.CodeValues on e.DesignationGroup equals f.CodeValue
                              join g in dBContext.CodeValues on b.Gender equals g.CodeValue
                              join pst in dBContext.CodeValues on b.PState equals pst.CodeValue
                              join cst in dBContext.CodeValues on b.CState equals cst.CodeValue
                              where a.UserId == userId
                              select new EmployeeUserProfileResponseDTO()
                              {
                                  BranchId = d.BranchId,
                                  UniqueId = a.UniqueId,                                 
                                  UserId = a.UserId,
                                  FullName = $"{b.FirstName} {b.MiddleName} {b.LastName}",                                 
                                  Designation = e.Title,
                                  BranchName = d.BranchName,
                                  ContactNumber = b.MobileNo,
                                  MobileNo = b.MobileNo,
                                  DOB = b.DOB,
                                  EmployeeId = b.EmployeeId,
                                  EmailId = a.EmailId,
                                  EmployeeName = $"{b.FirstName} {b.MiddleName} {b.LastName}",
                                  Gender = g.CodeValDescription,
                                  ProfileImageUrl = a.Photo,
                                  PermanentAddress = $"{b.PermanentAddress}, {pst.CodeValDescription}, Pincode - {b.PPincode}",
                                 CorrespondenceAddress = $"{b.CorrespondenceAddress}, {cst.CodeValDescription}, Pincode - {b.CPincode}"
                              }).FirstOrDefaultAsync();
            default:
                return null;
        }
    }
}
