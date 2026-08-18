namespace edumis.Models.Alumni.Members.DTO;

public class AlumniSearchResponseDTO
{
    public Guid AlumniId { get; set; }
    public string? DOERegistrationId { get; set; }   
    public string SalutationTitle { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DOB { get; set; }  
    public string GenderTitle { get; set; } = default!;
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }
    public string? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool BranchNotInList { get; set; }
    public string? OtherBranchName { get; set; }
    public string EmailID { get; set; } = default!;
    public string? MobileNo { get; set; }
    public bool IsResidentOfDelhi { get; set; }
    public string? CurrentProfession { get; set; }
    public byte[]? ProfileImage { get; set; }
    public string? ProfileImageExtn { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? ImageUrl { get; set; }
    public bool ShowOnHomePage { get; set; }
}
