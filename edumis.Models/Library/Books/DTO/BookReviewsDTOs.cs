using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Books.DTO;

public record BookReviewsRequestDTO(
    [Required] Guid BookId,
    [Required] string ReviewText,
    int? Rating
      
);

public record BookReviewsUpdateRequestDTO(
    [Required] Guid BookId,
    [Required] Guid ReviewId,
    [Required] string ReviewText,
    int? Rating
);

public record BookReviewsApproveRequestDTO(
    [Required] Guid BookId,
    [Required] Guid ReviewId,
    [Required] bool IsApproved
);

public class BookReviewsDetailsDTO
{
    public Guid ReviewId { get; set; }
    public string ReviewText { get; set; } = default!;
    public int? Rating { get; set; }
    public string? ReviewerID { get; set; }
    public bool IsApproved { get; set; }
}
