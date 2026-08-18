using edumis.Models.Masters.DTO;
using Npgsql;

namespace edumis.DataAccess.Mappers
{
    internal class InfrastructureMapper
    {
        public static List<InfrastructureDetailsDTO> ToInfraList(NpgsqlDataReader reader)
        {
            if (reader == null)
                return null;
            var ReturnModel = new List<InfrastructureDetailsDTO>();

            while (reader.Read())
            {
                var DTOModel = new InfrastructureDetailsDTO(
                    Convert.IsDBNull(reader["buildingid"]) ? string.Empty : reader["buildingid"].ToString(),
                    Convert.IsDBNull(reader["buildingname"]) ? string.Empty : reader["buildingname"].ToString(),
                    Convert.IsDBNull(reader["location"]) ? string.Empty : reader["location"].ToString(),
                    Convert.IsDBNull(reader["longitude"]) ? string.Empty : reader["longitude"].ToString(),
                    Convert.IsDBNull(reader["latitude"]) ? string.Empty : reader["latitude"].ToString(),
                    Convert.IsDBNull(reader["landowning"]) ? 0 : Convert.ToInt32(reader["landowning"].ToString()),
                    Convert.IsDBNull(reader["landowningtitle"]) ? string.Empty : reader["landowningtitle"].ToString(),
                    Convert.IsDBNull(reader["totalfloors"]) ? 0 : Convert.ToInt32(reader["totalfloors"].ToString()),
                    Convert.IsDBNull(reader["totalarea"]) ? 0 : Convert.ToInt32(reader["totalarea"].ToString()),
                    Convert.IsDBNull(reader["fencing"]) ? false : Convert.ToBoolean(reader["fencing"].ToString()),
                    Convert.IsDBNull(reader["tinshed"]) ? false : Convert.ToBoolean(reader["tinshed"].ToString()),
                    Convert.IsDBNull(reader["park"]) ? false : Convert.ToBoolean(reader["park"].ToString()),
                    Convert.IsDBNull(reader["totaltrees"]) ? 0 : Convert.ToInt32(reader["totaltrees"].ToString()),
                    Convert.IsDBNull(reader["waterharvesting"]) ? false : Convert.ToBoolean(reader["waterharvesting"].ToString()),
                    Convert.IsDBNull(reader["drinkingwater"]) ? false : Convert.ToBoolean(reader["drinkingwater"].ToString()),
                    Convert.IsDBNull(reader["toiletfacility"]) ? false : Convert.ToBoolean(reader["toiletfacility"].ToString()),
                    Convert.IsDBNull(reader["handicapramp"]) ? false : Convert.ToBoolean(reader["handicapramp"].ToString()),
                    Convert.IsDBNull(reader["cyclestand"]) ? false : Convert.ToBoolean(reader["cyclestand"].ToString()),
                    Convert.IsDBNull(reader["vehicleparking"]) ? false : Convert.ToBoolean(reader["vehicleparking"].ToString()),
                    Convert.IsDBNull(reader["accommodation"]) ? false : Convert.ToBoolean(reader["accommodation"].ToString()),
                    Convert.IsDBNull(reader["badmintoncourt"]) ? false : Convert.ToBoolean(reader["badmintoncourt"].ToString()),
                    Convert.IsDBNull(reader["tthall"]) ? false : Convert.ToBoolean(reader["tthall"].ToString()),
                    Convert.IsDBNull(reader["basketballcourt"]) ? false : Convert.ToBoolean(reader["basketballcourt"].ToString()),
                    Convert.IsDBNull(reader["shootingrange"]) ? false : Convert.ToBoolean(reader["shootingrange"].ToString()),
                    Convert.IsDBNull(reader["swimmingpool"]) ? false : Convert.ToBoolean(reader["swimmingpool"].ToString()),
                    Convert.IsDBNull(reader["boxingarena"]) ? false : Convert.ToBoolean(reader["boxingarena"].ToString()),
                    Convert.IsDBNull(reader["wrestlingarena"]) ? false : Convert.ToBoolean(reader["wrestlingarena"].ToString()),
                    Convert.IsDBNull(reader["runningtrack"]) ? false : Convert.ToBoolean(reader["runningtrack"].ToString()),
                    Convert.IsDBNull(reader["weightliftinghall"]) ? false : Convert.ToBoolean(reader["weightliftinghall"].ToString()),
                    Convert.IsDBNull(reader["lawnteniscourt"]) ? false : Convert.ToBoolean(reader["lawnteniscourt"].ToString()),
                    Convert.IsDBNull(reader["archeryground"]) ? false : Convert.ToBoolean(reader["archeryground"].ToString()),
                    Convert.IsDBNull(reader["openingyear"]) ? 0 : Convert.ToInt32(reader["openingyear"].ToString()),
                    Convert.IsDBNull(reader["maintenanceagency"]) ? string.Empty : reader["maintenanceagency"].ToString(),
                    Convert.IsDBNull(reader["isactive"]) ? false : Convert.ToBoolean(reader["isactive"].ToString())
                );

                ReturnModel.Add(DTOModel);
            }
            return ReturnModel;
        }
    }
}
