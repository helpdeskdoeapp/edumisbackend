using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Leave;

[Table("tble_leave_application_track")]
public class LeaveApplicationTrackModel{
    [Column("rowid")]
    public long RowId { get; set; }
    [Column("application_id")]
    public required string ApplicationId { get; set; }
    [Column("action_by")]
    public required string ActionBy { get; set; }
    [Column("action_type")]
    public required string ActionType { get; set; } // Created, Approved, Rejected, Reverted, Forwarded
    [Column("comment")]
    public string? Comment { get; set; }
    [Column("action_at")]
    public DateTime ActionAt { get; set; }
    [Column("ipaddress")]
    public string? IpAddress { get; set; }

}
