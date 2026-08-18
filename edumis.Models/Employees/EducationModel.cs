using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Employees;

[Table("tbemp_educationdetails")]
public class EducationModel : BaseEntity<long>
{  
    [Column(name: "employeeid", TypeName = "varchar(50)")]
    public string EmployeeId { get; set; } = default!;

    [Column(name: "serialno")]
    public int SerialNo { get; set; }
       
    [Column(name: "qualification")]
    public int Qualification { get; set; }
       
    [Column(name: "title", TypeName = "varchar(500)")]    
    public string Title { get; set; } = default!;
       
    [Column(name: "issuedate", TypeName = "date")]
    public DateOnly IssueDate { get; set; }

    [Column(name: "board", TypeName = "varchar(500)")]    
    public string? Board { get; set; }

    [Column(name: "percentage")]
    public decimal? Percentage { get; set; }

    [Column(name: "grade", TypeName = "varchar(10)")]    
    public string? Grade { get; set; }

    [Column(name: "subjects", TypeName = "varchar(1000)")]    
    public string? Subjects { get; set; }

    public EmployeeModel EmployeeNavigation { get; set; } = default!;
}
