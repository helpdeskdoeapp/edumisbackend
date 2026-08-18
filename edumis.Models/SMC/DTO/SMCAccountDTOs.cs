namespace edumis.Models.SMC.DTO;

public class LoginDTO
{
    public required string UserName { get; set; }

    public required string Password { get; set; }        
}

public record SMCDeptUserLoginDTO(
    string UniqueId,
    string MobileNo,
    int MemberType
);

public class VerifyBranchOtpRequest
{
    public string Otp { get; set; }
}
   
public class SMCUserDTO
{
    public Guid UserId { get; set; }
    public string UniqueId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string BranchId { get; set; } = default!;
    public string Branch { get; set; } = default!;
    public string Designation { get; set; } = default!;
    public int usertype { get; set; }
    //public int UserRole { get; set; }
    public bool? IsAccountLocked { get; set; }
    public bool? IsValid { get; set; }
    public bool? IsLoggedIn { get; set; }
}

public class SMCBranchDetailsDTO
{
    public Guid UserId { get; set; }  
    public string BranchId { get; set; } = default!;
    public string BranchName { get; set; } = default!;   
    public int BranchType { get; set; }
    public int UserType { get; set; }
    public string UserTypeDesc { get; set; } = default!;
    public bool IsValid { get; set; }  
}
