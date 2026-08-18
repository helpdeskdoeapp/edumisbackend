namespace edumis.Models.Users.DTO.UserProfile;

public class BaseUserProfileDTO
{    public Guid UserId { get; set; }
    public string UniqueId { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? EmailId { get; set; }
    public string? MobileNo { get; set; }
    public string? ContactNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? ProfileImageExtn { get; set; }
}
