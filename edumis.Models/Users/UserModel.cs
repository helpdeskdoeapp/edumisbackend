using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Users;

[Table("tbmslogin")]
public class UserModel : BaseEntity<long>
{       
    [Column(name: "userid")]
    public Guid UserId { get; set; }
        
    [Column(name: "uniqueid", TypeName = "varchar(50)")]
    public string UniqueId { get; set; } = default!;      
        
    [Column(name: "emailid", TypeName = "varchar(200)")]
    [EmailAddress]
    public string? EmailId { get; set; }

    [Column(name: "is_email_verified")]
    public bool? IsEmailVerified { get; set; } 
       
    [Column(name: "usertype")]
    public int UserType { get; set; }

    [Column(name: "userrole")]
    public int UserRole { get; set; }

    [Column(name: "password", TypeName = "varchar(150)")]
    public string Password { get; set; } = default!;

    [Column(name: "prevpassword1", TypeName = "varchar(150)")]
    public string? PrevPassword1 { get; set; }

    [Column(name: "prevpassword2", TypeName = "varchar(150)")]
    public string? PrevPassword2 { get; set; }

    [Column(name: "lastpwdchangeddate")]
    public DateTime? LastPwdChangedDate { get; set; }

    [Column(name: "ispwdchangewarningsent")]
    public bool? IsPwdChangeWarningSent { get; set; }

    [Column(name: "maxnoofinvalidloginattempt")]
    public int? MaxNoOfInvalidLoginAttempt { get; set; }

    [Column(name: "isaccountlocked")]
    public bool? IsAccountLocked { get; set; }

    [Column(name: "isvalid")]
    public bool IsValid { get; set; }

    [Column(name: "isloggedin")]
    public bool? IsLoggedIn { get; set; }

    [Column(name: "photo", TypeName = "varchar(250)")]
    public string? Photo { get; set; }

    //public EmployeeModel EmployeeNavigation { get; set; } = default!;
}
