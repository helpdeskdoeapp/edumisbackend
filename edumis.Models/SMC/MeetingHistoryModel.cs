using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_meeting_hist")]
public class MeetingHistoryModel
{    
    [Column("rowid")]   
    public long RowID { get; set; }

    [Column(name: "meetingid")] 
    public Guid MeetingId { get; set; }

    [Column(name: "fieldname", TypeName = "varchar(200)")]  
    public string FieldName { get; set; } = default!;

    [Column(name: "amendmentno")]   
    public int AmendmentNo { get; set; }

    [Column(name: "fieldvalue", TypeName = "text")]    
    public string FieldValue { get; set; } = default!;

    [Column(name: "createdby", TypeName = "varchar(200)")]
    public string? CreatedBy { get; set; }

    [Column(name: "createddate")]
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

}
