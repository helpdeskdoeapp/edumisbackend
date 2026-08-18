using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Newspaper;

[Table("tbbk_newspaper")]
public class NewspaperModel : BaseEntity<long>
{
    [Column("newspaperid", TypeName = "uuid")]
    public Guid NewspaperId { get; set; }

    [Column("branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column("Title", TypeName = "varchar(250)")]
    public string Title { get; set; } = default!;

    [Column("language")]
    public int Language { get; set; }

    [Column("frequency")]
    public int Frequency { get; set; }

    [Column("genre", TypeName = "integer[]")]
    public int[]? Genre { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("ebookurl", TypeName = "varchar(250)")]
    public string? EBookUrl { get; set; }

    [Column("price", TypeName = "numeric")]
    public decimal? Price { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("isactive")]
    public bool IsActive { get; set; }
}
