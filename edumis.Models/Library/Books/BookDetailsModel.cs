using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Library.Books;

[Table("tbbk_bookdetails")]
public class BookDetailsModel : BaseEntity<long>
{
    [Column("bookid", TypeName = "uuid")]
    public Guid BookId { get; set; }

    [Column("branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column("booklevel")]
    public int BookLevel {  get; set; }

    [Column("booktype")]
    public int BookType {  get; set; } 

    [Column("volumeno")]
    public int? VolumeNumber { get; set; } = default!;

    [Column("title", TypeName ="varchar(250)")]
    public string Title { get; set; } = default!;

    [Column("subtitle", TypeName = "varchar(250)")]
    public string SubTitle { get; set; } = default!;

    [Column("author_first_name", TypeName = "varchar(150)")]
    public string? AuthorFirstName { get; set; }

    [Column("author_mid_name", TypeName = "varchar(150)")]
    public string? AuthorMiddleName { get; set; }

    [Column("author_last_name", TypeName = "varchar(150)")]
    public string? AuthorLastName { get; set; }

    [Column("publisher", TypeName = "varchar(250)")]
    public string? Publisher { get; set; }

    [Column("editor", TypeName = "varchar(250)")]
    public string? Editor { get; set; }

    [Column("classcode")]
    public int? ClassCode { get; set; }

    [Column("subject")]
    public int? Subject { get; set; }

    [Column("language")]
    public int Language { get; set; }

    [Column("genre", TypeName = "integer[]")]
    public int[]? Genre { get; set; }

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Column("ddcno", TypeName = "varchar(150)")]
    public string? DDCNo { get; set; } = default!;

    [Column("subdivisionno", TypeName = "varchar(150)")]
    public string? SubdivisionNo { get; set; } = default!;

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

    [Column("relatedbooks", TypeName = "uuid[]")]
    public Guid[]? RelatedBooks { get; set; }

    [Column("awards", TypeName = "jsonb")]
    public string? Awards { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("availableqty")]
    public int AvailableQty { get; set; }

    [Column("isbn", TypeName = "varchar(50)")]
    public string? ISBN { get; set; }

    [Column("callno", TypeName = "varchar(250)")]
    public string? CallNumber { get; set; }

    [Column("edition", TypeName = "varchar(250)")]
    public string? Edition { get; set; }

    [Column("publication_year")]
    public int? PublicationYear { get; set; }

    [Column("no_of_pages")]
    public int? NumberOfPages { get; set; }

    public IList<BookCatalogueModel> BookCatalogueList { get; set; } = default!;
    public ICollection<ProcurementTransactionModel>? BookProcurementTransactionList { get; set; } = default!;
    public ICollection<BookReviewsModel>? BookReviewsList { get; set; } = default!;
}
