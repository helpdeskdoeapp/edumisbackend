using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Events.DTO;

public record EventRequestDTO
(   
    [Required] string Title,
    string? Description,
    [Required] string Venue,
    [Required] int Category,
    [Required] DateOnly StartDate,
    [Required] DateOnly EndDate,
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime,
    string? OrganizedBy,
    string? BranchId,
    string? VideoLink,
    string? ExternalLink,      
    bool? AlumniEvent
);

public record EventUpdateRequestDTO
(
    [Required] long RecordId,
    [Required] string Title,
    string? Description,
    [Required] string Venue,
    [Required] int Category,
    [Required] DateOnly StartDate,
    [Required] DateOnly EndDate,
    [Required] TimeOnly StartTime,
    [Required] TimeOnly EndTime,
    string? OrganizedBy,   
    string? VideoLink,
    string? ExternalLink,
    bool? AlumniEvent
);

public class SearchEventsRequestDTO
{
    public string? ForSession { get; set; }
    public string? BranchId { get; set; }
    public int? Category { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class EventResponseDTO
{
    public long RecordId { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string Venue { get; set; } = default!;
    public int Category { get; set; }
    public string CategoryDesc { get; set; } = default!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? OrganizedBy { get; set; } = default!;
    public string? BranchId { get; set; }
    public string? VideoLink { get; set; }
    public string? ExternalLink { get; set; }
    public string? BannerFilePath { get; set; }
    public string? BannerFileName { get; set; }
    public string? BannerFileExtn { get; set; }
    public string? BannerFileContentType { get; set; }
    public bool IsValid { get; set; } = true;
    public bool? AlumniEvent { get; set; }
}
