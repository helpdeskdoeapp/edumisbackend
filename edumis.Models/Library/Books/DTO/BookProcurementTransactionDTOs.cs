using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Books.DTO;

public record BookProcurementTransactionRequestDTO(  
    [Required] int ProcurementSource,
    DateOnly? ProcurementDate,
    string? OtherProcurementSource,
    string? BillNo,
    DateOnly? BillDate,
    decimal? BillAmount,
    decimal? Price,
    [Required] int Quantity
);

public record BookProcurementUpdateRequestDTO(
    [Required] Guid BookId,
    [Required] int ProcurementSource,
    DateOnly? ProcurementDate,
    string? OtherProcurementSource,
    string? BillNo,
    DateOnly? BillDate,
    decimal? BillAmount,
    decimal? Price,
    [Required] int Quantity
);

public class BookProcurementTransactionDetailsDTO
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