using edumis.Models.SMC.DTO;
using Npgsql;
using System.Reflection;
using System.Xml.Linq;

namespace edumis.DataAccess.Mappers.SMC
{
    internal class SMCEmployeeMapper
    {
        public static List<SMCMemberDetailsDTO> ToEmployeeDetails(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<SMCMemberDetailsDTO>();

            while (reader.Read())
            {
                var DTOModel = new SMCMemberDetailsDTO(
                    Convert.IsDBNull(reader["memberid"]) ? string.Empty : reader["memberid"].ToString(),
                    Convert.IsDBNull(reader["uniqueid"]) ? string.Empty : reader["uniqueid"].ToString(),
                    Convert.IsDBNull(reader["name"]) ? string.Empty : reader["name"].ToString(),
                    Convert.IsDBNull(reader["designationid"]) ? 0 : Convert.ToInt32(reader["designationid"].ToString()),
                    Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                    Convert.IsDBNull(reader["membertype"]) ? 0 : Convert.ToInt32(reader["membertype"].ToString()),
                    Convert.IsDBNull(reader["membertypedesc"]) ? string.Empty : reader["membertypedesc"].ToString(),
                    Convert.IsDBNull(reader["gender"]) ? 0 : Convert.ToInt32(reader["gender"].ToString()),
                    Convert.IsDBNull(reader["gendertitle"]) ? string.Empty : reader["gendertitle"].ToString(),                    
                    Convert.IsDBNull(reader["mobileno"]) ? string.Empty : reader["mobileno"].ToString(),
                    Convert.IsDBNull(reader["branchid"]) ? string.Empty : reader["branchid"].ToString(),
                    Convert.IsDBNull(reader["branchname"]) ? string.Empty : reader["branchname"].ToString(),
                    Convert.IsDBNull(reader["isactive"]) ? false : Convert.ToBoolean(reader["isactive"].ToString()),
                    Convert.IsDBNull(reader["forsession"]) ? string.Empty : reader["forsession"].ToString()
                );               

                ReturnModel.Add(DTOModel);
            }
            return ReturnModel;
        } 

        public static List<SMCMemberDetailsDTO> ToSearchResult(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<SMCMemberDetailsDTO>();

            while (reader.Read())
            {
                var DTOModel = new SMCMemberDetailsDTO(
                    Convert.IsDBNull(reader["memberid"]) ? string.Empty : reader["memberid"].ToString(),
                    Convert.IsDBNull(reader["uniqueid"]) ? string.Empty : reader["uniqueid"].ToString(),
                    Convert.IsDBNull(reader["name"]) ? string.Empty : reader["name"].ToString(),
                    Convert.IsDBNull(reader["designationid"]) ? 0 : Convert.ToInt32(reader["designationid"].ToString()),
                    Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                    Convert.IsDBNull(reader["membertype"]) ? 0 : Convert.ToInt32(reader["membertype"].ToString()),
                    Convert.IsDBNull(reader["membertypedesc"]) ? string.Empty : reader["membertypedesc"].ToString(),
                    Convert.IsDBNull(reader["gender"]) ? 0 : Convert.ToInt32(reader["gender"].ToString()),
                    Convert.IsDBNull(reader["gendertitle"]) ? string.Empty : reader["gendertitle"].ToString(),
                    Convert.IsDBNull(reader["mobileno"]) ? string.Empty : reader["mobileno"].ToString(),
                    Convert.IsDBNull(reader["branchid"]) ? string.Empty : reader["branchid"].ToString(),
                    Convert.IsDBNull(reader["branchname"]) ? string.Empty : reader["branchname"].ToString(),
                    Convert.IsDBNull(reader["isactive"]) ? false : Convert.ToBoolean(reader["isactive"].ToString()),
                    Convert.IsDBNull(reader["forsession"]) ? string.Empty : reader["forsession"].ToString()
                );

                ReturnModel.Add(DTOModel);
            }
            return ReturnModel;
        }
    }
}
