using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_transactions")]
public class SMCFundTransactionsModel : BaseEntity<long>
{
    [Column("transactionid", TypeName = "uuid")]
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    [Column("meetingid", TypeName = "uuid")]
    public Guid? MeetingId { get; set; }

    [Column("resolutionid", TypeName = "uuid")]
    public Guid ResolutionId { get; set; } = default!;   

    [Column("transdate", TypeName = "date")]
    public DateOnly TransactionDate {  get; set; }

    [Column("transmode")]
    public int TransactionMode { get; set; }

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = default!;

    [Column("refdocno", TypeName = "varchar(150)")]
    public string? ReferenceDocNo {  get; set; }

    [Column("amount", TypeName = "numeric")]
    public decimal Amount {  get; set; }
    
    [Column("is_active", TypeName = "boolean")]
    public bool IsActive { get; set; } = true;
    
    [Column("remarks", TypeName = "text")]
    public string? Remarks { get; set; } = null;

    //[ForeignKey(nameof(MeetingId))]
    public MeetingModel MeetingNavigation { get; set; } = default!;
    public IList<SMCTransactionAttachmentsModel> SMCTransactionAttachmentsList { get; private set; } = default!;
}
