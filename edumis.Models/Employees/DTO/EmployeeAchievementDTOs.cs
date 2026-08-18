using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Employees.DTO;

public record EmployeeAchievementRequestDTO(
    [Required] string EmployeeId,
    [Required] string Achievement,
    [Required] bool IsActive
);

public class EmployeeAchievementUpdateDTO
{
    [Required] public long RecordId { get; set; }
    [Required] public string EmployeeId { get; set; } = default!;
    [Required] public string Achievement { get; set; } = default!;    
    [Required] public bool IsActive { get; set; }
}

public class EmployeeAchievementDTO
{
    public long RecordID { get; set; }
    public string EmployeeId { get; set; } = default!;
    public string EmployeeName { get; set; } = default!;
    public string Designation { get; set; } = default!;
    public string Achievement { get; set; } = default!;
    public string? FileUploaded { get; set; }
    public string? FileExtension { get; set; }
    public string? FileContentType { get; set; }
    public string? FilePath { get; set; }
    public bool IsActive { get; set; }
}
