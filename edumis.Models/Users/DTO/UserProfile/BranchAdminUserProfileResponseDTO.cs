namespace edumis.Models.Users.DTO.UserProfile;

public class BranchAdminUserProfileResponseDTO : BaseUserProfileDTO
{
    public string? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? Address { get; set; }   
    public string? District { get; set; }
    public string? Zone { get; set; }
    public string? InchargeId { get; set; }
    public string? InchargeName { get; set; }
}
