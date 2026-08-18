using edumis.DataAccess.DBHelper;
using edumis.DataAccess.IRepositories.IMasters;
using edumis.DataAccess.Mappers;
using edumis.Models;
using edumis.Models.Masters.DTO;
using NpgsqlTypes;
using System.Data;

namespace edumis.DataAccess.Repositories.Masters;

internal class InfrastructureRepo : Repository<Models.Masters.InfrastructureModel>, IInfrastructureRepo
{
    private readonly ApplicationDBContext dBContext;
    public InfrastructureRepo(ApplicationDBContext dBContext) : base(dBContext)
    {
        this.dBContext = dBContext;
    }

    public async Task<string> CreateOrUpdate(InfrastructureDTO infrastructureDetails, bool CreateNew = false)
    {
        var spParamList = new ParamHelper
            {
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_buildingid", DBType= NpgsqlDbType.Varchar, ParamValue = (CreateNew ? "" : infrastructureDetails.BuildingId) },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_buildingname", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.BuildingName },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_location", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.Location },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_longitude", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.Longitude },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_latitude", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.Latitude },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_landowning", DBType= NpgsqlDbType.Integer, ParamValue = infrastructureDetails.LandOwning },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_totalfloors", DBType= NpgsqlDbType.Integer, ParamValue = infrastructureDetails.TotalFloors },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_totalarea", DBType= NpgsqlDbType.Integer, ParamValue = infrastructureDetails.TotalArea },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_fencing", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.Fencing },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_tinshed", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.TinShed },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_park", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.park },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_totaltrees", DBType= NpgsqlDbType.Integer, ParamValue = infrastructureDetails.TotalTrees },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_waterharvesting", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.WaterHarvesting },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_drinkingwater", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.DrinkingWater },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_toiletfacility", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.ToiletFacility },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_handicapramp", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.HandicapRamp },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_cyclestand", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.CycleStand },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_vehicleparking", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.VehicleParking },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_accommodation", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.Accommodation },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_badmintoncourt", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.BadmintonCourt },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_tthall", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.TTHall },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_basketballcourt", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.BasketBallCourt },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_shootingrange", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.ShootingRange },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_swimmingpool", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.SwimmingPool },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_boxingarena", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.BoxingArena },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_wrestlingarena", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.WrestlingArena },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_runningtrack", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.RunningTrack },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_weightliftinghall", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.WeightLiftingHall },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_lawnteniscourt", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.LawnTenisCourt },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_archeryground", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.ArcheryGround },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_openingyear", DBType= NpgsqlDbType.Integer, ParamValue = infrastructureDetails.OpeningYear },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_maintenanceagency", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.MaintenanceAgency },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_isactive", DBType= NpgsqlDbType.Boolean, ParamValue = infrastructureDetails.IsActive },
                new NpgSqlParam { ParamDirection = ParameterDirection.Input, ParamName = "p_userid", DBType= NpgsqlDbType.Varchar, ParamValue = infrastructureDetails.LoggedInUserId}
            };

        try
        {
            ErrorModel? error = null;
            object? ReturnVal = await ExecNonQueryTransSingle(@"select * from spms_infrainsupd(
                                                                    :p_buildingid,
                                                                    :p_buildingname,
                                                                    :p_location,
                                                                    :p_longitude,
                                                                    :p_latitude,
                                                                    :p_landowning,
                                                                    :p_totalfloors,
                                                                    :p_totalarea,
                                                                    :p_fencing ,
                                                                    :p_tinshed ,
                                                                    :p_park ,
                                                                    :p_totaltrees ,
                                                                    :p_waterharvesting ,
                                                                    :p_drinkingwater ,
                                                                    :p_toiletfacility ,
                                                                    :p_handicapramp ,
                                                                    :p_cyclestand ,
                                                                    :p_vehicleparking ,
                                                                    :p_accommodation ,
                                                                    :p_badmintoncourt ,
                                                                    :p_tthall ,
                                                                    :p_basketballcourt ,
                                                                    :p_shootingrange ,
                                                                    :p_swimmingpool ,
                                                                    :p_boxingarena ,
                                                                    :p_wrestlingarena ,
                                                                    :p_runningtrack ,
                                                                    :p_weightliftinghall ,
                                                                    :p_lawnteniscourt ,
                                                                    :p_archeryground ,
                                                                    :p_openingyear,
                                                                    :p_maintenanceagency,
                                                                    :p_isactive ,
                                                                   :p_userid)", spParamList, error);
            return ReturnVal != null ? (string)ReturnVal : string.Empty;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<List<InfrastructureDetailsDTO>> GetAllInfra()
    {
        ErrorModel error = null;
        return await ExecuteSPReader(@"select * from spms_searchinfrastructures()", null, InfrastructureMapper.ToInfraList, error);
    }
}
