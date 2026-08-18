namespace edumis.Models.SMC.DTO;

public class MeetingResolutionDetailsDTO
{
    public Guid ResolutionId { get; set; }
    public Guid MeetingId { get; set; }    
    public string Resolution { get; set; } = default!;
    public bool? IsClosed { get; set; }
    public DateOnly? ClosingDate { get; set; }
    public string? Comments { get; set; } = default!;
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public DateTime? CreatedDate { get; set; }
    public IList<string>? AgendaList { get; set; } = default!;
    public IList<SmcFundTransactionShortDto>? Transactions { get; set; } = default!;  
}