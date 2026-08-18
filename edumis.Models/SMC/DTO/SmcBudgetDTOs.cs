using System.ComponentModel.DataAnnotations;

namespace edumis.Models.SMC.DTO;

public record SmcBudgetNewAllocationDto(
    string? Session,
    string SchoolId,
    decimal Amount,
    int AllocationType,
    DateTime? AllocationDate ,
    string? DonorName = null, 
    string? DonorPan = null,
    string? DonorMobile = null,
    string? DonorAddress = null,
    string? Remarks = null
);

public record SmcBudgetAllocationDetailDto(
    string Session,
    string SchoolId,
    decimal Allocation,
    decimal Consumption
    );

public record SmcBudgetHistoryEntry {
    [Required] public string Session;
    [Required] public string SchoolId;
    [Required] public decimal Amount;
    [Required] public int AllocationType;
    [Required] public string AllocationTypeDesc;
    [Required] public DateTime AllocationDate;
    public string? DonorName = null;
    public string? DonorPan = null;
    public string? DonorMobile = null;
    public string? DonorAddress = null;
    public string? Remarks = null;
};