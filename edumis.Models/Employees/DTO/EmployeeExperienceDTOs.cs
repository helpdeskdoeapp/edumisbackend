using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Employees.DTO;

public record EmployeeExperienceRequestDTO
(
    [Required] string EmployeeId,
    [Required] string Experience,
    [Required] bool IsActive
);

public class EmployeeExperienceUpdateDTO
{
    [Required] public long RecordId { get; set; }
    [Required] public string EmployeeId { get; set; } = default!;
    [Required] public string Experience { get; set; } = default!;   
    [Required] public bool IsActive { get; set; }
}

public class EmployeeExperienceDTO
{
    public long RecordId { get; set; }
    public string EmployeeId { get; set; } = default!;
    public string EmployeeName { get; set; } = default!;
    public string Designation { get; set; } = default!;
    public string Experience { get; set; } = default!;
    public string? FileUploaded { get; set; } = default!;
    public string? FilePath { get; set; } = default!;
    public string? FileExtension { get; set; }
    public string? FileContentType { get; set; }
    public bool IsActive { get; set; }
}
