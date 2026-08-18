using edumis.Models.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace edumis.Models.Leave;

[Table("tble_leave_applications")]
public class LeaveApplicationModel{
    [Required] [Column("application_id")]
    public string ApplicationId { get; set; }
   
    [Required] [Column("applied_at")]
    public DateTime AppliedAt { get; set; }
    
    [Required]
    [Column("employee_id")]
    public required string EmployeeId {  get; set; }
    
    [Column("deputed_branch_id")] public string? DivertedBranchId { get; set; } = null;

    [Column("service_branch_id")] public string? ServiceBranchId { get; set; } = null;

    [Column("zone_id")] public string? ZoneId { get; set; } = null;

    [Column("district_id")] public string? DistrictId { get; set; } = null;

    [Column("region_id")] public string? RegionId { get; set; } = null;

    [Column("goc_id")] public string? GocId { get; set; } = null;

    [Column("hq_id")] public string? HqBranchId { get; set; } = null;

    [Required]
    [Column("leave_type")]
    public required int LeaveType { get; set; }

    [Column("from_date")]
    public DateOnly FromDate { get; set; }
    
    [Column("to_date")]
    public DateOnly ToDate { get; set; }

    [Column("days")]
    public int Days { get; set; }

    [Column("address_during_leave", TypeName = "varchar(500)")]
    public string? AddressDuringLeave { get; set; }

    [Column("leave_station", TypeName = "varchar(500)")]
    public required string LeaveStation { get; set; }

    [Column("with_noc")] public bool? LeaveWithNoc { get; set; } = null;

    [Column("child_dob")] public DateOnly? ChildDob { get; set; } = null;

    [Column("leave_status")] public required LeaveStatus LeaveStatus { get; set; } = LeaveStatus.Pending;

    [Column("current_level")] public LeaveLevel CurrentLevel { get; set; } = 0;

    [Column("reason")] public required string Reason { get; set; } = "";
    [Column("updated_at")] public required DateTime UpdatedAt { get; set; }
    [Column("updated_by")] public required string UpdatedBy { get; set; }
    
    

}
