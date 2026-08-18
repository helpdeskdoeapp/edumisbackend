using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Alumni.UserAccounts;

[Table("tbalm_login")]
public class AlumniLoginModel : BaseEntity<long>
{
    [Column("alumni_id", TypeName = "uuid")]
    public Guid AlumniID { get; set; } = default!;
        
    [Column("emailid", TypeName = "varchar(150)")]
    public string EmailID { get; set; } = default!;

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

    public AlumniLoginModel(){}

    public AlumniLoginModel(Guid alumniId, string emailId, string password)
    {
        AlumniID = alumniId;
        EmailID = emailId;
        Password = password;
        IsPwdChangeWarningSent = false;
        MaxNoOfInvalidLoginAttempt = 5;
        LastPwdChangedDate = DateTime.UtcNow;
        IsAccountLocked = false;
        IsLoggedIn = false;
        IsValid = true;
    }

    public void SetPassword(string password, string userID) {
        Password = password;
        PrevPassword1 = Password;
        PrevPassword2 = PrevPassword1;
        IsPwdChangeWarningSent = false;
        MaxNoOfInvalidLoginAttempt = 5;
        LastPwdChangedDate = DateTime.UtcNow;
        ModifiedBy = userID;
        ModifiedDate = DateTime.UtcNow;
    }
}
