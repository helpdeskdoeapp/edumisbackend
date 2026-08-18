using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbms_subjects")]
public class AcademicSubjectsModel : BaseEntity<int>
{
    [Column(name: "title", TypeName = "varchar(150)")]
    public string Title { get; set; } = default!;

    [Column(name: "subject_code", TypeName = "varchar(50)")]
    public string? SubjectCode { get; set; } = default!;

    [Column(name: "isactive")]
    public bool IsActive { get; set; }
}
