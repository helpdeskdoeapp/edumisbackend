using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbms_classes")]
public class AcademicClassesModel : BaseEntity<int>
{
    [Column(name: "title", TypeName = "varchar(150)")]
    public string Title { get; set; } = default!;

    [Column(name: "class_code", TypeName = "varchar(50)")]
    public string? ClassCode { get; set; } = default!;

    [Column(name: "sections")]
    public int[]? Sections { get; set; } = default!;

    [Column(name: "isactive")]
    public bool IsActive { get; set; }
}
