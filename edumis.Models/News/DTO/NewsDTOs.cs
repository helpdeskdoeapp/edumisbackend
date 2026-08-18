using System.ComponentModel.DataAnnotations;

namespace edumis.Models.News.DTO;

public record NewsRequestDTO
(
    [Required] string Title,
    string? Description,
    string? VideoLink,
    string? ExternalLink,
    [Required] DateOnly NewsDate,
    bool? AlumniNews
);

public record NewsUpdateRequestDTO
(
    [Required] long RecordId,
    [Required] string Title,
    string? Description,
    string? VideoLink,
    string? ExternalLink,
    [Required] DateOnly NewsDate,
    bool? AlumniNews
);

public class SearchNewsRequestDTO
{
    public string? ForSession { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public bool? AlumniNews {  get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public record NewsDetailResponseDTO
{
    public long RecordId { get; set; }
    public string FinancialYear { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? Photo { get; set; }
    public string? VideoLink { get; set; }
    public string? ExternalLink { get; set; }
    public DateOnly NewsDate { get; set; }
    public string? BannerFilePath { get; set; }
    public string? BannerFileName { get; set; }
    public string? BannerFileExtn { get; set; }
    public string? BannerFileContentType { get; set; }
    public bool IsValid { get; set; }
    public bool? AlumniNews { get; set; }
}
