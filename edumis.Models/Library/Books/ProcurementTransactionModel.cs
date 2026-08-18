using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Books;

[Table("tbbk_procure_trans")]
public class ProcurementTransactionModel : BaseEntity<long>
{
    [Column("bookid", TypeName = "uuid")]
    public Guid BookId { get; set; }

    [Column("transid", TypeName = "uuid")]
    public Guid TransactionId { get; set; }

    [Column("procurement_source")]
    public int ProcurementSource { get; set; }

    [Column("procurementdate", TypeName = "date")]
    public DateOnly? ProcurementDate { get; set; }

    [Column("other_procurement_src", TypeName = "varchar(250)")]
    public string? OtherProcurementSource { get; set; }

    [Column("billno", TypeName = "varchar(150)")]
    public string? BillNo { get; set; }

    [Column("billdate", TypeName = "date")]
    public DateOnly? BillDate { get; set; }

    [Column("billamount", TypeName = "numeric")]
    public decimal? BillAmount { get; set; }

    [Column("price", TypeName = "numeric")]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int Quantity {  get; set; }

    public BookDetailsModel BookDetailsNavigation { get; set; } = default!;
}
