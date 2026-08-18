using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Masters.DTO;

public record BranchRequestDTO(
    [MaxLength(50)][Required] string BranchId,
    [MaxLength(100)] string? BuildingId,
    [Required][MaxLength(500)] string BranchName,
    [Required] int BranchType,
    [MaxLength(50)] string? ParentBranchId,
    int? DistrictId,
    int? ZoneId,   
    string? InchargeId,
    string? EmailId,
    string? ContactNo,
    string? Address
);

public record SearchBranchRequestDTO(
    int? DistrictId,
    int? ZoneId,
    int? BranchType,
    bool? Status,
    string? BuildingId,
    string? BranchId,
    int PageNumber = 1,
    int PageSize = 10
);

public class BranchDetailsDTO
{
    public string BranchId { get; set; } = default!;
    public string? BuildingId { get; set; }
    public string BranchName { get; set; } = default!;
    public int BranchType { get; set; }
    public string BranchTypeDesc { get; set; } = default!;
    public string? ParentBranchId { get; set; }
    public int? DistrictId { get; set; } 
    public string? DistrictTitle { get; set; }
    public int? ZoneId { get; set; }
    public string? ZoneTitle { get; set; }
    public string? InchargeId { get; set; }
    public string? InchargeName { get; set; } 
    public string? EmailId { get; set; }
    public string? ContactNo { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}
public class BranchesNamesDTO
{
    public string BranchId { get; set; } = default!;  
    public string BranchName { get; set; } = default!;   
    public int BranchType { get; set; }
}