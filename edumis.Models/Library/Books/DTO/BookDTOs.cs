using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Books.DTO;

public record BookRequestDTO(
    [Required] int BookLevel,
    [Required] int BookType,
    int? VolumeNumber,
    [Required] string Title,
    string SubTitle,
    string? AuthorFirstName,
    string? AuthorMiddleName,
    string? AuthorLastName,
    string? Publisher, 
    string? Editor, 
    int? ClassCode, 
    int? Subject,
    [Required] int Language, 
    int[]? Genre, 
    string? Description,
    string? DDCNo,
    string? SubdivisionNo,    
    string? Notes,
    string? Tags,
    //int? Rating,
    string? EBookUrl,
    string? AudioUrl,
    string? VideoUrl,
    //Guid[]? RelatedBooks,
    //string? Awards,
    [Required] string ISBN,
    string? CallNumber,
    string? Edition,
    int? PublicationYear,
    int? NumberOfPages,   
    //BookCatalogueRequestDTO BookCatalogueDetails,
    BookProcurementTransactionRequestDTO BookProcurementDetails
);

public record BookUpdateRequestDTO(
    [Required] Guid BookId,
    [Required] int BookLevel,
    [Required] int BookType,
    int? VolumeNumber,
    [Required] string Title,
    string SubTitle,
    string? AuthorFirstName,
    string? AuthorMiddleName,
    string? AuthorLastName,
    string? Publisher,
    string? Editor,
    int? ClassCode,
    int? Subject,
    [Required] int Language,
    int[]? Genre,
    string? Description,
    string? DDCNo,
    string? SubdivisionNo,
    string? Notes,
    string? Tags,
    //int? Rating,
    string? EBookUrl,
    string? AudioUrl,    
    [Required] string ISBN,
    string? CallNumber,
    string? Edition,
    int? PublicationYear,
    int? NumberOfPages
);

public class BookDetailsDTO
{
    public Guid BookId { get; set; }
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public int BookLevel { get; set; }
    public string BookLevelDesc { get; set; } = default!;
    public int BookType { get; set; }
    public string BookTypeDesc { get; set; } = default!;
    public int? VolumeNumber { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string SubTitle { get; set; } = default!;
    public string? AuthorFirstName { get; set; }
    public string? AuthorMiddleName { get; set; }
    public string? AuthorLastName { get; set; }
    public string? Publisher { get; set; }
    public string? Editor { get; set; }
    public int? ClassCode { get; set; }
    public string? ClassCodeDesc { get; set; } = default!;
    public int? Subject { get; set; }
    public string? SubjectDesc { get; set; } = default!;
    public int Language { get; set; }
    public string LanguageDesc { get; set; } = default!;    
    public string? Description { get; set; }
    public string? DDCNo { get; set; } = default!;
    public string? SubdivisionNo { get; set; } = default!;
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public int? Rating { get; set; }
    public string? EBookUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? VideoUrl { get; set; }    
    public string? Awards { get; set; }
    public int Qty { get; set; }
    public int AvailableQty { get; set; }
    public string? ISBN { get; set; }
    public string? CallNumber { get; set; }
    public string? Edition { get; set; }
    public int? PublicationYear { get; set; }
    public int? NumberOfPages { get; set; }
    public IDictionary<int, string>? Genre { get; set; }
    public IList<RelatedBookDetailsDTO>? RelatedBooks { get; set; }
    public IList<BookCatalogueDetailsDTO>? CatalogueDetails { get; set; }
    public IList<BookProcurementTransactionDetailsDTO>? BookProcurementTransactions { get; set; } = default!;
    public IList<BookReviewsDetailsDTO> BookReviewsDetails { get; set; } = default!;    
}

public class RelatedBookDetailsDTO
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = default!;
    public string SubTitle { get; set; } = default!;
    public string? AuthorFirstName { get; set; }
    public string? AuthorMiddleName { get; set; }
    public string? AuthorLastName { get; set; }    
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public int? Rating { get; set; }
}

public record SearchBookRequestDTO(   
    int? ClassCode,
    int? Subject,
    int? Language,
    int? BookLevel,
    int? BookType,
    string[]? Tags,
    string? Title,
    string? SubTitle,
    string? Author,
    string? Publisher,
    string? Editor,
    int? Rating,
    int PageNumber = 1,
    int PageSize = 10
);

public class BookSearchResultDTO
{
    public Guid BookId { get; set; }
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string SubTitle { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public int BookLevel { get; set; }
    public string BookLevelDesc { get; set; } = default!;
    public int BookType { get; set; }
    public string BookTypeDesc { get; set; } = default!;
    public int? VolumeNumber { get; set; } = default!;
    public string? AuthorFirstName { get; set; }
    public string? AuthorMiddleName { get; set; }
    public string? AuthorLastName { get; set; }
    public string? Publisher { get; set; }
    public string? Editor { get; set; }
    public int? ClassCode { get; set; }
    public string? ClassCodeDesc { get; set; } = default!;
    public int? Subject { get; set; }
    public string? SubjectDesc { get; set; } = default!;
    public int Language { get; set; }
    public string LanguageDesc { get; set; } = default!;
    public string? Description { get; set; }
    public string? DDCNo { get; set; } = default!;
    public string? SubdivisionNo { get; set; } = default!;
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public int? Rating { get; set; }
    public int Qty { get; set; }
    public int AvailableQty { get; set; }
}