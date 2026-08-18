using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_trans_attachments")]
public class SMCTransactionAttachmentsModel : BaseEntity<long>
{
    [Column("transactionid")]
    public Guid TransactionId { get; set; }

    [Column("serialno")]
    public int SerialNo {  get; set; }

    [Column(name: "title", TypeName = "varchar(500)")]
    public string Title { get; set; } = default!;

    [Column(name: "filename", TypeName = "varchar(500)")]
    public string FileName { get; set; } = default!;

    [Column(name: "contenttype", TypeName = "varchar(100)")]
    public string? ContentType { get; set; } = default!;

    [Column(name: "extension", TypeName = "varchar(50)")]
    public string? Extension { get; set; } = default!;

    [Column(name: "filepath", TypeName = "varchar(500)")]
    public string? FilePath { get; set; } = default!;

    public SMCFundTransactionsModel SMCFundTransactionNavigation { get; set; } = default!;
}
