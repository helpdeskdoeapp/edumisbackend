using edumis.Models.Global;
using edumis.Models.Masters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_meeting")]
public class MeetingModel : BaseEntity<long>
{
    [Column("meetingid")]   
    public Guid MeetingId { get; set; } = Guid.NewGuid();

    [Column(name: "forsession", TypeName = "varchar(10)")] 
    public string ForSession { get; set; } = default!;

    [Column(name: "branchid", TypeName = "varchar(50)")]   
    public string BranchId { get; set; } = default!;

    [Column(name: "meetingdate", TypeName = "date")]    
    public DateOnly MeetingDate { get; set; }

    [Column(name: "meetingtime", TypeName = "time")]   
    public TimeOnly MeetingTime { get; set; }

    [Column(name: "title", TypeName = "varchar(500)")]  
    public string Title { get; set; } = default!;
 
    [Column(name: "invitees", TypeName = "varchar[]")]        
    public string[]? Invitees { get; set; } = default!;

    [Column(name: "attendees", TypeName = "varchar[]")]
    public string[]? Attendees { get; set; }

    [Column(name: "mom_brief", TypeName = "text")]       
    public string? Mom_Brief { get; set; }

    //[Column(name: "mom_attachment", TypeName = "varchar(250)")]
    //public string? Mom_Attachment { get; set; }
   
    [Column(name: "status")]
    public int Status { get; set; }

    [ForeignKey(nameof(BranchId))]
    public BranchesModel? BranchesNavigation { get; set; }

    [ForeignKey(nameof(ForSession))]
    public SessionInfoModel? SessionNavigation { get; set; }
    //public IList<SMCFundTransactionsModel> SMCFundTransactionsModels { get; private set; } = default!;

    //public IList<MeetingAgendaModel> MeetingAgendaList { get; private set; } = default!;
}
