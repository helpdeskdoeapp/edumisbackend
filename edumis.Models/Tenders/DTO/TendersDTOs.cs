using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Tenders.DTO;

public class TendersDetailsResponseDTO
{
    public long RecordId { get; set; }
    public string FinancialYear { get; set; } = default!;
    public DateTime TenderDate { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public string? FileExtn { get; set; }
    public string? FileContentType { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public TimeOnly ExpiryTime { get; set; }
    public bool IsValid { get; set; }        
}

public class TenderRequestDTO
{
    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public DateOnly TenderDate { get; set; }

    public string Description { get; set; } = default!;
               
    [Required]
    public DateOnly ExpiryDate { get; set; }

    [Required]
    public TimeOnly ExpiryTime { get; set; }
}


public class TenderUpdateRequestDTO
{
    [Required]
    public long RecordId { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    [Required]
    public DateOnly TenderDate { get; set; }

    public string Description { get; set; } = default!;

    [Required]
    public DateOnly ExpiryDate { get; set; }

    [Required]
    public TimeOnly ExpiryTime { get; set; }
}

public class SearchTendersRequestDTO
{
    public string? ForSession { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}