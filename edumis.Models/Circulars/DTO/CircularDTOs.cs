using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Circulars.DTO;

public record CircularRequestDataDTO( 
    [Required] DateOnly CircularDate,
    [Required] string Title,
    string? Description,
    [Required] int Type   
);

public record CircularUpdateRequestDTO(
    [Required] long RecordId,
    [Required] DateOnly CircularDate,
    [Required] string Title,
    string? Description,
    [Required] int Type
);

public class SearchCircularsRequestDTO
{   
    public string? ForSession { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }   
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CircularsDetailResponseDTO
{
    public long RecordId { get; set; }
    public string FinancialYear { get; set; } = default!;
    public DateOnly CircularDate { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int Type { get; set; }
    public string TypeDesc { get; set; } = default!;
    public bool IsValid { get; set; }
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? FileExtn { get; set; }
    public string? FileContentType { get; set; }
}
