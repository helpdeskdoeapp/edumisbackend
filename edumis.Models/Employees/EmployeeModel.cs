using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Employees;

[Table("tbemp_employees")]
public class EmployeeModel : BaseEntity<long>
{    
    [Column(name: "employeeid", TypeName = "varchar(50)")]
    public string EmployeeId { get; set; } = default!;
        
    [Column(name: "firstname", TypeName = "varchar(250)")]
    public string FirstName { get; set; } = default!;
        
    [Column(name: "middlename", TypeName = "varchar(250)")]
    public string? MiddleName { get; set; }
        
    [Column(name: "lastname", TypeName = "varchar(250)")]
    public string? LastName { get; set; }
        
    [Column(name: "fathername", TypeName = "varchar(250)")]
    public string? FatherName { get; set; }
      
    [Column(name: "mothername", TypeName = "varchar(250)")]
    public string? MotherName { get; set; }
        
    [Column(name: "gender")]
    public int? Gender { get; set; }
       
    [Column(name: "dob", TypeName = "date")]
    public DateOnly? DOB { get; set; }

    [Column(name: "aadharno", TypeName = "varchar(20)")]
    public string? AadharNo { get; set; }

    [Column(name: "panno", TypeName = "varchar(20)")]
    public string? PanNo { get; set; }
       
    [EmailAddress]
    [Column(name: "emailid", TypeName = "varchar(250)")]
    public string? EmailId { get; set; } = default!;

    [Column(name: "mobileno", TypeName = "varchar(10)")]
    public string? MobileNo { get; set; } = default!;
        
    [Column(name: "permanentaddress", TypeName = "varchar(500)")]
    public string? PermanentAddress { get; set; } = default!;

    [Column(name: "pcity", TypeName = "varchar(150)")]
    public string? PCity { get; set; } = default!;
        
    [Column(name: "pstate")]
    public int? PState { get; set; }
       
    [Column(name: "ppincode", TypeName = "varchar(10)")]
    public string? PPincode { get; set; } = default!;
       
    [Column(name: "correspondenceaddress", TypeName = "varchar(500)")]
    public string? CorrespondenceAddress { get; set; } = default!;

    [Column(name: "ccity", TypeName = "varchar(150)")]
    public string? CCity { get; set; } = default!;
        
    [Column(name: "cstate")]
    public int? CState { get; set; }

    [Column(name: "cpincode", TypeName = "varchar(10)")]
    public string? CPincode { get; set; } = default!;
       
    [Column(name: "category")]
    public int? Category { get; set; }

    [Column(name: "subcategory")]
    public int? SubCategory { get; set; }

    [Column(name: "highestqualification")]
    public int? HighestQualification { get; set; }

    [Column(name: "maritalstatus")]
    public int? MaritalStatus { get; set; }

    [Column(name: "isanydisability")]
    public bool? IsAnyDisability { get; set; }

    [Column(name: "disabilitytype")]
    public int? DisabilityType { get; set; }

    [Column(name: "otherdisabilitytype", TypeName = "varchar(150)")]
    public string? OtherDisabilityType { get; set; }

    [Column(name: "isgazetted")]
    public bool? IsGazetted { get; set; }

    [Column(name: "vehiclefacilityavailed")]
    public bool? VehicleFacilityAvailed { get; set; }

    [Column(name: "reportingpersonid", TypeName = "varchar(50)")]
    public string? ReportingPersonId { get; set; }

    [Column(name: "isactive")]
    public bool? IsActive { get; set; } = false;

    [Column(name: "remarks", TypeName = "varchar(500)")]
    public string? Remarks { get; set; }

    [Column(name: "photo", TypeName = "bytea")]
    public byte[]? Photo { get; set; }

    [Column(name: "extension", TypeName = "varchar(20)")]
    public string? Extension { get; set; }

    [Column(name: "contenttype", TypeName = "varchar(50)")]
    public string? ContentType { get; set; }

    //public UserModel UserNavigation { get; set; } = default!;
    public AppointmentModel? EmployeeAppointmentNavigation { get; set; } = default!;
    public ICollection<EducationModel>? EmployeeEducationList { get; set; } = default!;
    public ICollection<EmployeeAchievementModel>? EmployeeAchievementsList {  get; set; } = default!;
    public ICollection<EmployeeExperienceModel>? EmployeeExperiencesList { get; set; } = default!;

    public string Name => $"{FirstName} {LastName}";
}
