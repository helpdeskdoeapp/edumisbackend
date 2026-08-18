using System.Security.AccessControl;

namespace edumis.Models.Users.DTO;

public class UserProfileResponseDTO
{
    public Guid UserId { get; set; }  
    public string UniqueId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string? BranchId { get; set; }
    public string? BranchTitle { get; set; }
    public string? BranchAddress { get; set; }
    public string? BranchPhoneNumber { get; set; }
    public string? BranchEmailId { get; set; }  
    public string? Designation { get; set; }      
    public string? EmailId { get; set; }
    public string? MobileNo { get; set; }   
    public string? District {  get; set; }
    public string? Zone {  get; set; }
    public string? InchargeId { get; set; }
    public string? InchargeName { get; set; }
}
