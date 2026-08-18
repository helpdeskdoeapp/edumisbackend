using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Users.DTO;

public class UserDTO
{
    public Guid UserId { get; set; }
    public string UniqueId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? BranchId { get; set; }
    public string? BranchTitle { get; set; }
    public int? DesignationId { get; set; }
    public string? Designation { get; set; }
    public int? DesignationGroupId { get; set; }
    public string? DesignationGroup { get; set; }
    public int UserType { get; set; }
    public int? UserRole { get; set; }
    public bool? IsAccountLocked { get; set; }
    public bool? IsValid { get; set; }
    public bool? IsLoggedIn { get; set; }
    public string? DeviceToken { get; set; }
}

public record UserContactDetailsDTO(
     Guid UserId,
     string MobileNo
  );

public record PasswordResetRequestDTO(
         [Required] string UserId,
         [Required] string OTPText,
         [Required] string NewPassword,
         [Required] string IPAddress,
         [Required] string UserAgent
      );

public record UpdatePasswordRequestDTO(
     [Required] string CurrentPassword,
     [Required] string NewPassword,
     [Required] string IPAddress,
     [Required] string UserAgent
  );

public record UserDetailsDTO(
     Guid UserId,
     string UniqueId,
     string UserName,
     string Password,
     int UserType,
     int? UserRole,
     string EmailId,
     string MobileNo,
     bool IsValid,
     bool? IsAccountLocked,
     bool? IsLoggedIn
);