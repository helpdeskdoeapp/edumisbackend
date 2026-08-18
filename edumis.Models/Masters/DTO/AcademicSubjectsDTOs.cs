using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Masters.DTO;

public record AcademicSubjectsRequestDTO
(
    [Required] string Title,
    string? SubjectCode
);

public record AcademicSubjectsUpdateRequestDTO
(
    [Required] int RecordId,
    [Required] string Title,
    string? SubjectCode
);

public class AcademicSubjectsResponseDTO
{
    public int RecordId { get; set; }
    public string Title { get; set; } = default!;
    public string? SubjectCode { get; set; } = default!;
    public bool IsActive { get; set; }
}
