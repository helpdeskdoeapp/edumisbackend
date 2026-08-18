using edumis.Models.Masters.DTO;

namespace edumis.Models.Alumni.Members.DTO;

public class AlumniDetailsDTO
{
    public string? DOERegistrationId { get; set; }
    public int Salutation { get; set; }
    public string SalutationTitle { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DOB { get; set; }
    public int Gender { get; set; }
    public string GenderTitle { get; set; } = default!;
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }      
    public string? BranchId { get; set; }
    public string? BranchName { get; set; }
    public bool BranchNotInList { get; set; }
    public string? OtherBranchName { get; set; }
    public string EmailID { get; set; } = default!;
    public string? AlternateEmailId { get; set; }
    public string? CurrentOrganization { get; set; }
    public string? CurrentDesignation { get; set; }
    public string? CurrentResidence { get; set; }
    public string? ResidenceContactNo { get; set; }
    public string? WorkContactNo { get; set; }
    public string? MobileNo { get; set; }
    public string? CurrentResidenceCity { get; set; }
    public int? CurrentProfession { get; set; }
    public string? CurrentProfessionDesc { get; set; }
    public string? OtherProfession { get; set; }
    public bool IsResidentOfDelhi { get; set; }
    public byte[]? ProfileImage { get; set; }  
    public string? ProfileImageExtn { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? ImageUrl { get; set; }
    public bool ShowEmailID { get; set; }
    public bool ShowMobileNo { get; set; }
    public bool ShowCurrentOrganisation { get; set; }
    public bool ShowCurrentDesignation { get; set; }
    public bool ShowCurrentResidence { get; set; }
    public bool ShowResidenceContactNo { get; set; }
    public bool ShowWorkContactNo { get; set; }
    public bool ShowCurrentResidenceCity { get; set; }
    public bool ShowCurrentProfession { get; set; }   
    public bool ShowOnHomePage { get; set; }
}
