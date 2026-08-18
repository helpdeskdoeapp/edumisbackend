using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Employees;

[Table("tbemp_appointmentdetails")]
public class AppointmentModel :BaseEntity<long>
{  
    [Column(name: "employeeid", TypeName = "varchar(50)")]
    public string EmployeeId { get; set; } = default!;
        
    [Column(name: "designation")]
    public int Designation { get; set; }

    [Column(name: "seniorityno")]
    public int? SeniorityNo { get; set; }
       
    [Column(name: "appointmenttype")]
    public int AppointmentType { get; set; }

    [Column(name: "appointmentorder", TypeName = "varchar(200)")]
    public string? AppointmentOrder { get; set; }

    [Column(name: "appointmentdate", TypeName ="date")]
    public DateOnly? AppointmentDate { get; set; }
        
    [Column(name: "branchjoiningdate", TypeName = "date")]
    public DateOnly BranchJoiningDate { get; set; }
       
    [Column(name: "recruitmenttype")]
    public int RecruitmentType { get; set; }
        
    [Column(name: "currentpostheld")]
    public int CurrentPostHeld { get; set; }

    [Column(name: "currentbranch", TypeName = "varchar(50)")]
    public string CurrentBranch { get; set; } = default!;

    [Column(name: "cadre")]
    public int Cadre { get; set; }

    [Column(name: "currentscale", TypeName = "varchar(50)")]
    public string CurrentScale { get; set; } = default!;

    [Column(name: "grade", TypeName = "varchar(50)")]
    public string? Grade { get; set; }

    [Column(name: "gradegrantdate", TypeName = "date")]
    public DateOnly? GradeGrantDate { get; set; }

    [Column(name: "retirementdate", TypeName = "date")]
    public DateOnly? RetirementDate { get; set; }

    [Column(name: "selectioncategory")]
    public int? SelectionCategory { get; set; }

    public EmployeeModel EmployeeNavigation { get; set; } = default!;
}
