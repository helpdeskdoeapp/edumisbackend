using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_logins")]
public class SMCAccountsModel : BaseEntity<long>
{       
    [Column(name: "userid")]
    public Guid UserId { get; set; }   
       
    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;
      
    [Column(name: "usertype")]
    public int UserType { get; set; }
    
    [Column(name: "mobileno", TypeName = "varchar(10)")]
    public string? MobileNo { get; set; }

    [Column(name: "emailid", TypeName = "varchar(150)")]
    public string? EmailId { get; set; }

    [Column(name: "password", TypeName = "varchar(150)")]
    public string? Password { get; set; }

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
    public bool? IsValid { get; set; }

    [Column(name: "isloggedin")]
    public bool? IsLoggedIn { get; set; }

    [Column(name: "photo", TypeName = "bytea")]
    public byte[]? Photo { get; set; }        
}
