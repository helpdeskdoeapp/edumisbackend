using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Masters.DTO;

public record AcademicClassesRequestDTO
(
    [Required] string Title,
    string? ClassCode,
    int[]? Sections
);

public record AcademicClassesUpdateRequestDTO
(
    [Required] int RecordId,
    [Required] string Title,
    string? ClassCode,
    int[]? Sections
);

public class AcademicClassesResponseDTO
{
    public int RecordId { get; set; }
    public string Title { get; set; } = default!;
    public string? ClassCode { get; set; } = default!;
    public int[]? Sections { get; set; } = default!;
    public bool IsActive { get; set; }
}