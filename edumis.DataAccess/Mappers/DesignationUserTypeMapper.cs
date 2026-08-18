using edumis.Models.Global.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers
{
    internal class DesignationUserTypeMapper
    {
        public static List<DesignationUserTypeMappingDetailsDTO> ToMappings(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<DesignationUserTypeMappingDetailsDTO>();

            while (reader.Read())
            {
                var DTOModel = new DesignationUserTypeMappingDetailsDTO(
                    Convert.IsDBNull(reader["designationid"]) ? 0 : Convert.ToInt32(reader["designationid"].ToString()),
                    Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                    Convert.IsDBNull(reader["usertype"]) ? 0 : Convert.ToInt32(reader["usertype"].ToString()),
                    Convert.IsDBNull(reader["usertypedesc"]) ? string.Empty : reader["usertypedesc"].ToString()
                );
                ReturnModel.Add(DTOModel);
            }
            return ReturnModel;
        }
    }
}
