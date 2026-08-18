namespace edumis.Models.Users.DTO.UserProfile;

public class EmployeeUserProfileResponseDTO : BaseUserProfileDTO
{
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? Designation { get; set; }   
    public string? BranchId { get; set; }
    public string? BranchName { get; set; }
    public string? PermanentAddress { get; set; }
    public string? CorrespondenceAddress { get; set; }
    public string? Gender { get; set; }
    public DateOnly? DOB { get; set; }
}

