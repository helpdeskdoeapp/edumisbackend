using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Books;

[Table("tbbk_catalogue")]
public class BookCatalogueModel : BaseEntity<long>
{
    [Column("bookid", TypeName = "uuid")]
    public Guid BookId { get; set; }
    
    [Column("accessionno")]
    public string AccessionNumber { get; set; } = default!;

    [Column("accession_serialno")]
    public int AccessionSerialNo { get; set; }

    [Column("location", TypeName = "varchar(250)")]
    public string? Location { get; set; }

    [Column("shelf", TypeName = "varchar(250)")]
    public string? Shelf { get; set; }       

    [Column("condition")]
    public int Condition { get; set; }

    [Column("conditionnotes", TypeName = "text")]
    public string? ConditionNotes { get; set; }

    [Column("damagetype")]
    public int? DamageType { get; set; }    

    [Column("status")]
    public int Status { get; set; }

    public BookDetailsModel BookDetailsNavigation { get; set; } = default!;
}
