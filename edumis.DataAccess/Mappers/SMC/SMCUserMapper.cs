using edumis.Models.SMC.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers.SMC
{
    internal class SMCUserMapper
    {
        public static List<SMCUserDTO> ToUserDetails(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<SMCUserDTO>();

            while (reader.Read())
            {
                var DTOModel = new SMCUserDTO
                {
                    UserId = Convert.IsDBNull(reader["userid"]) ? (new Guid()) : new Guid(reader["userid"].ToString()),
                    UniqueId = Convert.IsDBNull(reader["uniqueid"]) ? string.Empty : reader["uniqueid"].ToString(),
                    UserName = Convert.IsDBNull(reader["name"]) ? string.Empty : reader["name"].ToString(),
                    BranchId = Convert.IsDBNull(reader["branchid"]) ? string.Empty : reader["branchid"].ToString(),
                    Branch = Convert.IsDBNull(reader["branch"]) ? string.Empty : reader["branch"].ToString(),
                    Designation = Convert.IsDBNull(reader["designationdesc"]) ? string.Empty : reader["designationdesc"].ToString(),
                    //UserRole = Convert.IsDBNull(reader["userrole"]) ? 0 : Convert.ToInt32(reader["userrole"].ToString()),
                    usertype = Convert.IsDBNull(reader["usertype"]) ? 0 : Convert.ToInt32(reader["usertype"].ToString()),
                    IsAccountLocked = false,
                    IsValid = true,
                    IsLoggedIn = true
                };

                ReturnModel.Add(DTOModel);
            }

            return ReturnModel;
        }
    }
}
