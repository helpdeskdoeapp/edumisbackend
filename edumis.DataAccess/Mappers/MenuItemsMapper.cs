using edumis.Models.Global.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers
{
    internal class MenuItemsMapper
    {
        //public static List<MenuDetailDTO> ToMenuItems(NpgsqlDataReader reader)
        //{
        //    if (reader == null) return null;

        //    var ReturnModel = new List<MenuDetailDTO>();
        //    while (reader.Read())
        //    {
        //        var DTOModel = new MenuDetailDTO(
        //            Convert.IsDBNull(reader["menuid"]) ? 0 : Convert.ToInt32(reader["menuid"].ToString()),
        //            Convert.IsDBNull(reader["menutitle"]) ? string.Empty : reader["menutitle"].ToString(),
        //            Convert.IsDBNull(reader["parentmenuid"]) ? 0 : Convert.ToInt32(reader["parentmenuid"].ToString()),
        //            Convert.IsDBNull(reader["parentmenutitle"]) ? string.Empty : reader["parentmenutitle"].ToString(),
        //            Convert.IsDBNull(reader["module"]) ? 0 : Convert.ToInt32(reader["module"].ToString()),
        //            Convert.IsDBNull(reader["moduletitle"]) ? string.Empty : reader["moduletitle"].ToString(),
        //            Convert.IsDBNull(reader["menuurl"]) ? string.Empty : reader["menuurl"].ToString(),
        //            Convert.IsDBNull(reader["isvalid"]) ? false : Convert.ToBoolean(reader["isvalid"].ToString())
        //        );
        //        ReturnModel.Add(DTOModel);
        //    }
        //    return ReturnModel;
        //}
    }
}
