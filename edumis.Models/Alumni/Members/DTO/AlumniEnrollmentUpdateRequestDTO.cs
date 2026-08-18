namespace edumis.Models.Alumni.Members.DTO;

public class AlumniEnrollmentUpdateRequestDTO
{
    public string? DOERegistrationId { get; set; }   
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }
    public string? BranchId { get; set; }    
    public string? OtherBranchName { get; set; }
}
