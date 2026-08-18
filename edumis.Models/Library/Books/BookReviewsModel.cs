using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Books;

[Table("tbbk_reviews")]
public class BookReviewsModel : BaseEntity<long>
{
    [Column("bookid", TypeName = "uuid")]
    public Guid BookId { get; set; }

    [Column("reviewid", TypeName = "uuid")]
    public Guid ReviewId { get; set; }

    [Column("reviewtext", TypeName = "text")]
    public string ReviewText { get; set; } = default!;

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("reviewerid", TypeName = "varchar(100)")]
    public string? ReviewerID { get; set; }

    [Column("isapproved")]
    public bool IsApproved { get; set; } = false;

    public BookDetailsModel BookDetailsNavigation { get; set; } = default!;
}
