using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbmsinfrastructure")]
public class InfrastructureModel : BaseEntity<int>
{
    [Column(name: "buildingid", TypeName = "varchar(50)")]
    public string BuildingId { get; set; } = default!;

    [Column(name: "buildingname", TypeName = "varchar(500)")]
    public string? BuildingName { get; set; }

  
    [Column(name: "location", TypeName = "varchar(500)")]
    public string? Location { get; set; }

    [Column(name: "longitude", TypeName = "varchar(50)")]
    public string? Longitude { get; set; }

    [Column(name: "latitude", TypeName = "varchar(50)")]
    public string? Latitude { get; set; }

    [Column(name: "landowning")]
    public int LandOwning { get; set; }

    [Column(name: "totalfloors")]
    public int? TotalFloors { get; set; }

    [Column(name: "totalarea")]
    public int? TotalArea { get; set; }

    [Column(name: "fencing")]
    public bool? Fencing { get; set; }

    [Column(name: "tinshed")]
    public bool? TinShed { get; set; }

    [Column(name: "park")]
    public bool? park { get; set; }

    [Column(name: "totaltrees")]
    public int? TotalTrees { get; set; }

    [Column(name: "waterharvesting")]
    public bool? WaterHarvesting { get; set; }

    [Column(name: "drinkingwater")]
    public bool? DrinkingWater { get; set; }

    [Column(name: "toiletfacility")]
    public bool? ToiletFacility { get; set; }

    [Column(name: "handicapramp")]
    public bool? HandicapRamp { get; set; }

    [Column(name: "cyclestand")]
    public bool? CycleStand { get; set; }

    [Column(name: "vehicleparking")]
    public bool? VehicleParking { get; set; }

    [Column(name: "accommodation")]
    public bool? Accommodation { get; set; }

    [Column(name: "badmintoncourt")]
    public bool? BadmintonCourt { get; set; }

    [Column(name: "tthall")]
    public bool? TTHall { get; set; }

    [Column(name: "basketballcourt")]
    public bool? BasketBallCourt { get; set; }

    [Column(name: "shootingrange")]
    public bool? ShootingRange { get; set; }

    [Column(name: "swimmingpool")]
    public bool? SwimmingPool { get; set; }

    [Column(name: "boxingarena")]
    public bool? BoxingArena { get; set; }

    [Column(name: "wrestlingarena")]
    public bool? WrestlingArena { get; set; }

    [Column(name: "runningtrack")]
    public bool? RunningTrack { get; set; }

    [Column(name: "weightliftinghall")]
    public bool? WeightLiftingHall { get; set; }

    [Column(name: "lawnteniscourt")]
    public bool? LawnTenisCourt { get; set; }

    [Column(name: "archeryground")]
    public bool? ArcheryGround { get; set; }

    [Column(name: "openingyear")]
    public int? OpeningYear { get; set; }

    [Column(name: "maintenanceagency")]
    public string? MaintenanceAgency { get; set; }

    [Column(name: "isactive")]
    public bool IsActive { get; set; }

    public ICollection<BranchesModel> ListBranches { get; set; } = default!;
}
