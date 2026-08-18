using System.ComponentModel.DataAnnotations;

namespace edumis.Models.SMC.DTO;

public record SMCFundTransactionRequestDTO
(
   // [Required] string MeetingId,
    [Required] string ResolutionId,
    [Required] DateOnly TransactionDate,
    [Required] string Description,
    string? ReferenceDocNo,
    [Required] decimal Amount,
    string? AttachmentTitle,
    [Required] int TransactionMode
);

public record AddTransactionAttachmentRequestDTO(
   [Required] string TransactionId,
   [Required] string AttachmentTitle
);
public record TransactionDeactivateDto(string? Remarks);
public class SMCFundTransactionDetailDTO
{   
    public Guid TransactionId { get; set; }
    public Guid? MeetingId { get; set; }
    public Guid ResolutionId { get; set; }
    public DateOnly TransactionDate { get; set; }
    public int TransactionMode {  get; set; }
    public string TransactionModeDesc { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ReferenceDocNo { get; set; }
    public decimal Amount { get; set; }
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }
    public DateOnly? LastModifiedDate { get; set; }
    public IList<SMCTransactionAttachmentListDTO> SMCTransactionAttachmentsList { get; set; } = default!;
}

public class SmcFundTransactionShortDto
{   
    public Guid TransactionId { get; set; }
    public Guid? MeetingId { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }
    public bool IsActive { get; set; }
}

public class SMCTransactionAttachmentListDTO
{
    public int SerialNo { get; set; }
    public string Title { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string? ContentType { get; set; } = default!;
    public string? Extension { get; set; } = default!;
    public string? FilePath { get; set; } = default!;
}