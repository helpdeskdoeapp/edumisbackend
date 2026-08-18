using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Newspaper.DTO;

public  record NewspaperRequestDTO
(
    [Required] string Title,
    [Required] int Language,
    [Required] int Frequency,
    int[]? Genre,
    string? Description,
    string? EBookUrl,
    decimal? Price,
    [Required] int Quantity
);

public record NewspaperUpdateRequestDTO
(
    [Required] string NewspaperId,
    [Required] string Title,
    [Required] int Language,
    [Required] int Frequency,
    int[]? Genre,
    string? Description,
    string? EBookUrl,
    decimal? Price,
    [Required] int Quantity
);

public record NewspaperDetailsResponseDTO
{
    public Guid NewspaperId { get; set; }
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Language { get; set; }
    public string LanguageDesc { get; set; } = default!;
    public int Frequency { get; set; }
    public string FrequencyDesc { get; set; } = default!;
    public IDictionary<int, string>? Genre { get; set; }
    public string? Description { get; set; }
    public string? EBookUrl { get; set; }
    public decimal? Price { get; set; }
    public int Quantity { get; set; }
    public bool IsActive { get; set; }
}
