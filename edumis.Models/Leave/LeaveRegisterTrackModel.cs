using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Leave;

[Table("tble_leave_register_track")]
public class LeaveRegisterTrackModel{
    [Column("rowid")]
    public int RowId { get; set; }
    
    [Column("employee_id")]
    public required string EmployeeId { get; set; }
    
    [Column("leave_type")]
    public required int LeaveType {  get; set; }
    
    [Column("action_by")]
    public required string ActionBy {  get; set; }
    
    [Column("action_type")]
    public required string ActionType { get; set; }
    
    [Column("action_at")]
    public required DateTime ActionAt {  get; set; }
    
    [Column("days")]
    public required float Days{ get; set; }
    
    [Column("comment")]
    public string? Comment { get; set; }
    
    [Column("leave_application_id")]
    public string? LeaveApplicationId { get; set; }
    
    [Column("ipaddress")]
    public string? IpAddress { get; set; }
}

