using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Magazine;

[Table("tbbk_magazine_procure_trans")]
public class MagazineProcurementTransactionModel : BaseEntity<long>
{
    [Column("magazineid", TypeName = "uuid")]
    public Guid MagazineId { get; set; }

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
    public int Quantity { get; set; }

    public MagazineModel MagazineNavigation { get; set; } = default!;
}
