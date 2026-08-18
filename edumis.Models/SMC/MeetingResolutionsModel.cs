using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_meeting_resolutions")]
public class MeetingResolutionsModel : BaseEntity<long>
{
    [Column("resolutionid", TypeName ="uuid")]   
    public Guid ResolutionId { get; set; } = Guid.NewGuid();

    [Column("meetingid")]
    public Guid MeetingId { get; set; }      

    [Column("agenda_srno", TypeName ="integer[]")]
    public int[]? AgendaSrNo { get; set; }
        
    [Column(name: "resolution", TypeName = "Text")]
    [Required]
    public string Resolution { get; set; } = default!;

    [Column(name: "isclosed")]
    public bool? IsClosed { get; set; } = false;

    [Column(name: "closingdate", TypeName = "date")]
    public DateOnly? ClosingDate { get; set; }

    [Column(name: "comments", TypeName = "Text")]   
    public string? Comments { get; set; } = default!;

    [Column(name: "estimatedcost", TypeName = "numeric")]
    public decimal? EstimatedCost { get; set; }

    [Column(name: "actualcost", TypeName = "numeric")]
    public decimal? ActualCost { get; set; }

    //[ForeignKey(nameof(MeetingId))]
    public MeetingModel? MeetingNavigation { get; set; }
}
