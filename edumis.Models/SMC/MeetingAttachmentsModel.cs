using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_meeting_attachments")]
public class MeetingAttachmentsModel : BaseEntity<long>
{   
    [Column("meetingid")]
    public Guid MeetingId { get; set; }

    [Column("serialno")]
    public int SerialNo { get; set; }

    [Column(name: "title", TypeName = "varchar(500)")]
    public string? Title {  get; set; } = default!;

    [Column(name: "filename", TypeName = "varchar(500)")] 
    public string? FileName { get; set; } = default!;

    [Column(name: "contenttype", TypeName = "varchar(100)")]
    public string? ContentType { get; set; } = default!;

    [Column(name: "extension", TypeName = "varchar(50)")]    
    public string? Extension {  get; set; } = default!;

    [Column(name: "filepath", TypeName = "varchar(500)")]
    public string? FilePath { get; set; } = default!;

    //[ForeignKey(nameof(MeetingId))]
    public MeetingModel? MeetingNavigation { get; set; }
}
