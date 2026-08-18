using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglacademicsessions")]
public class SessionInfoModel : BaseEntity<int>
{    
    [Column(name: "forsession", TypeName = "varchar(10)")]
    public string ForSession { get; set; } = default!;
       
    [Column(name: "isvalid")]
    public bool IsValid { get; set; }
        
    [Column(name: "iscurrent")]
    public bool IsCurrent { get; set; }

    [Column(name: "registrationstartdate", TypeName = "date")]
    public DateOnly? RegistrationStartDate { get; set; }

    [Column(name: "registrationenddate", TypeName = "date")]
    public DateOnly? RegistrationEndDate { get; set; }

    [Column(name: "lateregistrationstartdate", TypeName = "date")]
    public DateOnly? LateRegistrationStartDate { get; set; }

    [Column(name: "lateregistrationenddate", TypeName = "date")]
    public DateOnly? LateRegistrationEndDate { get; set; }

    [Column(name: "registrationendtime", TypeName = "time")]
    public TimeOnly? RegistrationEndTime { get; set; }

    [Column(name: "lateregistrationendtime", TypeName = "time")]
    public TimeOnly? LateRegistrationEndTime { get; set; }
        
    [Column(name: "reg_ageasondate", TypeName = "date")]
    public DateOnly? Reg_AgeAsOnDate { get; set; }

    [Column(name: "registrationstarttime", TypeName = "time")]
    public TimeOnly? RegistrationStartTime { get; set; }

    [Column(name: "lateregistrationstarttime", TypeName = "time")]
    public TimeOnly? LateRegistrationStartTime { get; set; }
       
    [Column(name: "isregistrationopen")]
    public bool IsRegistrationOpen { get; set; }
}
