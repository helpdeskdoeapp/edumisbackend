using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Magazine.DTO;

#region Magazine DTOs
public record MagazineRequestDTO
(
    [Required] string Title, 
    string? Publisher,
    string? Editor,
    string? Edition,
    [Required] int Language,
    [Required] int Frequency,
    int[]? Genre,
    string? Description,
    string? Notes,
    string? Tags,
    string? EBookUrl,
    string? AudioUrl,
    string? VideoUrl,
    MagazineProcurementTransactionRequestDTO ProcurementDetails
);

public record MagazineUpdateRequestDTO
(
    [Required] string MagazineId,
    [Required] string Title,
    string? Publisher,
    string? Editor,
    string? Edition,
    [Required] int Language,
    [Required] int Frequency,
    int[]? Genre,
    string? Description,
    string? Notes,
    string? Tags,
    string? EBookUrl,
    string? AudioUrl,
    string? VideoUrl
);


public class MagazineDetailsReponseDTO
{
    public Guid MagazineId { get; set; }
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Publisher { get; set; }
    public string? Editor { get; set; }
    public string? Edition {  get; set; }
    public int Language { get; set; }
    public string LanguageDesc { get; set; } = default!;
    public int Frequency { get; set; }
    public string FrequencyDesc { get; set; } = default!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public int? Rating { get; set; }
    public string? EBookUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? VideoUrl { get; set; }
    public int TotalQty { get; set; }
    public int AvailableQty { get; set; }
    public IDictionary<int, string>? Genre { get; set; }
    public IList<RelatedMagazinesDetailsDTO>? RelatedMagazines { get; set; }
    public IList<MagazineProcurementTransactionDetailsDTO>? MagazineProcurementTransactions { get; set; } = default!;
}

public class RelatedMagazinesDetailsDTO
{
    public Guid MagazineId { get; set; }
    public string Title { get; set; } = default!;
    public string? Publisher { get; set; }
    public string? Editor { get; set; }   
    public string? Edition {  get; set; }
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public int? Rating { get; set; }
}
#endregion

#region Magazine Procurement Transaction DTOs
public record MagazineProcurementTransactionRequestDTO
(
    [Required] int ProcurementSource,
    DateOnly? ProcurementDate,
    string? OtherProcurementSource,
    string? BillNo,
    DateOnly? BillDate,
    decimal? BillAmount,
    decimal? Price,
    [Required] int Quantity
);

public record MagazineProcurementUpdateRequestDTO
(    
    [Required] Guid MagazineId,
    [Required] int ProcurementSource,
    DateOnly? ProcurementDate,
    string? OtherProcurementSource,
    string? BillNo,
    DateOnly? BillDate,
    decimal? BillAmount,
    decimal? Price,
    [Required] int Quantity
);

public class MagazineProcurementTransactionDetailsDTO
{
    public Guid TransactionId { get; set; }
    public int ProcurementSource { get; set; }
    public string ProcurementSourceDesc { get; set; } = default!;
    public DateOnly? ProcurementDate { get; set; }
    public string? OtherProcurementSource { get; set; }
    public string? BillNo { get; set; }
    public DateOnly? BillDate { get; set; }
    public decimal? BillAmount { get; set; }
    public decimal? Price { get; set; }
    public int Quantity { get; set; }
}
#endregion

#region Search DTOs
public record SearchMagazineRequestDTO(
    int? Language,    
    string[]? Tags,
    string? Title,      
    string? Publisher,
    string? Editor,
    int? Rating,
    int PageNumber = 1,
    int PageSize = 10
);

public class MagazineSearchResultDTO
{
    public Guid MagazineId { get; set; }
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Publisher { get; set; }
    public string? Editor { get; set; }
    public string? Edition { get; set; }
    public int Language { get; set; }
    public string LanguageDesc { get; set; } = default!;
    public int Frequency { get; set; }
    public string FrequencyDesc { get; set; } = default!;
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? CoverImageExtenstion { get; set; }
    public string? CoverImageContentType { get; set; }
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public int? Rating { get; set; }
    public string? EBookUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? VideoUrl { get; set; }
    public int Qty { get; set; }
    public int AvailableQty { get; set; }
}
#endregion