using edumis.Models.Users.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers;

internal class UserMapper
{
    public static List<UserDTO> ToUserDetails(NpgsqlDataReader reader)
    {
        if (reader == null)
            return null;
        var ReturnModel = new List<UserDTO>();

        while (reader.Read())
        {
            var DTOModel = new UserDTO
            {
                UserId = Convert.IsDBNull(reader["userid"]) ? (new Guid()) : new Guid(reader["userid"].ToString()),
                UniqueId = Convert.IsDBNull(reader["uniqueid"]) ? string.Empty : reader["uniqueid"].ToString(),
                UserName = (Convert.IsDBNull(reader["firstname"]) ? string.Empty : reader["firstname"].ToString()) +
                    (Convert.IsDBNull(reader["middlename"]) ? string.Empty : " " + reader["middlename"].ToString()) +
                    (Convert.IsDBNull(reader["lastname"]) ? string.Empty : " " + reader["lastname"].ToString()),
                BranchId = Convert.IsDBNull(reader["branchid"]) ? string.Empty : reader["branchid"].ToString(),
                BranchTitle = Convert.IsDBNull(reader["branch"]) ? string.Empty : reader["branch"].ToString(),
                DesignationId = Convert.IsDBNull(reader["designation"]) ? 0 : Convert.ToInt32(reader["designation"].ToString()),
                Designation = Convert.IsDBNull(reader["designationdesc"]) ? string.Empty : reader["designationdesc"].ToString(),
                DesignationGroupId = Convert.IsDBNull(reader["designationgroupid"]) ? 0 : Convert.ToInt32(reader["designationgroupid"].ToString()),
                DesignationGroup = Convert.IsDBNull(reader["designationgroup"]) ? string.Empty : reader["designationgroup"].ToString(),
                UserRole = Convert.IsDBNull(reader["userrole"]) ? 0 : Convert.ToInt32(reader["userrole"].ToString()),
                UserType = Convert.IsDBNull(reader["usertype"]) ? 0 : Convert.ToInt32(reader["usertype"].ToString()),
                IsAccountLocked = Convert.IsDBNull(reader["isaccountlocked"]) ? false : Convert.ToBoolean(reader["isaccountlocked"].ToString()),
                IsValid = Convert.IsDBNull(reader["isvalid"]) ? false : Convert.ToBoolean(reader["isvalid"].ToString()),
                IsLoggedIn = Convert.IsDBNull(reader["isloggedin"]) ? false : Convert.ToBoolean(reader["isloggedin"].ToString())
            };

            ReturnModel.Add(DTOModel);
        }

        return ReturnModel;
    }

    public static List<UserDetailsDTO> ToUser(NpgsqlDataReader reader)
    {
        if (reader == null)
            return null;
        var ReturnModel = new List<UserDetailsDTO>();

        while (reader.Read())
        {
            var DTOModel = new UserDetailsDTO
            (
                Convert.IsDBNull(reader["userid"]) ? (new Guid()) : new Guid(reader["userid"].ToString()),
                Convert.IsDBNull(reader["uniqueid"]) ? string.Empty : reader["uniqueid"].ToString(),
                (Convert.IsDBNull(reader["firstname"]) ? string.Empty : reader["firstname"].ToString()) +
                    (Convert.IsDBNull(reader["middlename"]) ? string.Empty : " " + reader["middlename"].ToString()) +
                    (Convert.IsDBNull(reader["lastname"]) ? string.Empty : " " + reader["lastname"].ToString()),
                Convert.IsDBNull(reader["password"]) ? string.Empty : reader["password"].ToString(),
                Convert.IsDBNull(reader["usertype"]) ? 0 : Convert.ToInt32(reader["usertype"].ToString()),
                Convert.IsDBNull(reader["userrole"]) ? 0 : Convert.ToInt32(reader["userrole"].ToString()),
                Convert.IsDBNull(reader["emailid"]) ? string.Empty : reader["emailid"].ToString(),
                Convert.IsDBNull(reader["mobileno"]) ? string.Empty : reader["mobileno"].ToString(),
                Convert.IsDBNull(reader["isvalid"]) ? false : Convert.ToBoolean(reader["isvalid"].ToString()),
                Convert.IsDBNull(reader["isaccountlocked"]) ? false : Convert.ToBoolean(reader["isaccountlocked"].ToString()),
                Convert.IsDBNull(reader["isloggedin"]) ? false : Convert.ToBoolean(reader["isloggedin"].ToString())
            );
            ReturnModel.Add(DTOModel);
        }
        return ReturnModel;
    }
}
