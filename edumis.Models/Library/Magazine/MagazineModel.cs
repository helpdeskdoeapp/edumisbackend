using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Magazine;

[Table("tbbk_magazine")]
public class MagazineModel : BaseEntity<long>
{
    [Column("magazineid", TypeName = "uuid")]
    public Guid MagazineId { get; set; }

    [Column("branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column("title", TypeName = "varchar(250)")]
    public string Title { get; set; } = default!;

    [Column("publisher", TypeName = "varchar(250)")]
    public string? Publisher { get; set; }

    [Column("editor", TypeName = "varchar(250)")]
    public string? Editor { get; set; }

    [Column("edition", TypeName = "varchar(100)")]
    public string? Edition { get; set; }

    [Column("language")]
    public int Language { get; set; }

    [Column("frequency")]
    public int Frequency { get; set; }

    [Column("genre", TypeName = "integer[]")]
    public int[]? Genre { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("coverimageurl", TypeName = "varchar(250)")]
    public string? CoverImageUrl { get; set; }

    [Column("coverimage_extn", TypeName = "varchar(50)")]
    public string? CoverImageExtenstion { get; set; }

    [Column("coverimage_contenttype", TypeName = "varchar(100)")]
    public string? CoverImageContentType { get; set; }

    [Column("notes", TypeName = "text")]
    public string? Notes { get; set; }

    [Column("tags", TypeName = "varchar(500)")]
    public string? Tags { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("ebookurl", TypeName = "varchar(250)")]
    public string? EBookUrl { get; set; }

    [Column("audiourl", TypeName = "varchar(250)")]
    public string? AudioUrl { get; set; }

    [Column("videourl", TypeName = "varchar(250)")]
    public string? VideoUrl { get; set; }

    [Column("related_magazines", TypeName = "uuid[]")]
    public Guid[]? RelatedMagazines { get; set; }

    [Column("totalqty")]
    public int TotalQty { get; set; }

    [Column("availableqty")]
    public int AvailableQty { get; set; }

    public ICollection<MagazineProcurementTransactionModel>? MagazineProcurementTransactionList { get; set; } = default!;
}
