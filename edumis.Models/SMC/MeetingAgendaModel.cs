using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_meeting_agenda")]
public class MeetingAgendaModel : BaseEntity<long>
{ 
    [Column("meetingid")]    
    public Guid MeetingId { get; set; }

    [Column("serialno")]
    public int SerialNo {  get; set; }

    [Column("agendacode")]
    public int AgendaCode {  get; set; }    

    [Column(name: "otherdetails", TypeName = "varchar(500)")]   
    public string? OtherDetails { get; set; } = default!;

    //[ForeignKey(nameof(MeetingId))]
    public MeetingModel? MeetingNavigation { get; set; }
}
