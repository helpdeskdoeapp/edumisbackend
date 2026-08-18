using edumis.Models.Global.DTO;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edumis.DataAccess.Mappers
{
    internal class DesignationMenuMapper
    {
        public static List<DesignationMenuItemsDetailsDTO> ToMappings(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<DesignationMenuItemsDetailsDTO>();

            while (reader.Read())
            {
                var DTOModel = new DesignationMenuItemsDetailsDTO(
                    Convert.IsDBNull(reader["designationid"]) ? 0 : Convert.ToInt32(reader["designationid"].ToString()),
                    Convert.IsDBNull(reader["designationtitle"]) ? string.Empty : reader["designationtitle"].ToString(),
                    Convert.IsDBNull(reader["menuid"]) ? 0 : Convert.ToInt32(reader["menuid"].ToString()),
                    Convert.IsDBNull(reader["menutitle"]) ? string.Empty : reader["menutitle"].ToString(),
                    Convert.IsDBNull(reader["canview"]) ? false : Convert.ToBoolean(reader["canview"].ToString()),
                    Convert.IsDBNull(reader["cancreate"]) ? false : Convert.ToBoolean(reader["cancreate"].ToString()),
                    Convert.IsDBNull(reader["canedit"]) ? false : Convert.ToBoolean(reader["canedit"].ToString()),
                    Convert.IsDBNull(reader["candelete"]) ? false : Convert.ToBoolean(reader["candelete"].ToString())
                );
                ReturnModel.Add(DTOModel);
            }
            return ReturnModel;
        }
    }
}
