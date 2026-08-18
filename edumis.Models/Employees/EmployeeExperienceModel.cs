using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Employees;

[Table("tbemp_experiences")]
public class EmployeeExperienceModel : BaseEntity<long>
{  

    [Column(name: "employeeid", TypeName = "varchar(50)")]
    public string EmployeeId { get; set; } = default!;

    [Column("serialno")]
    public int SerialNo { get; set; }

    [Column(name: "experience", TypeName = "text")]
    public string Experience { get; set; } = default!;

    [Column(name: "fileuploaded", TypeName = "varchar(250)")]
    public string? FileUploaded { get; set; }

    [Column(name: "fileextension", TypeName = "varchar(50)")]
    public string? FileExtension { get; set; }

    [Column(name: "filecontenttype", TypeName = "varchar(100)")]
    public string? FileContentType { get; set; }

    [Column(name: "filepath", TypeName = "varchar(250)")]
    public string? FilePath { get; set; }
       
    [Column(name: "isactive")]
    public bool IsActive { get; set; }
    public EmployeeModel EmployeeNavigation { get; set; } = default!;
}
