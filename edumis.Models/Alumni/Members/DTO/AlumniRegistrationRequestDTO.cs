namespace edumis.Models.Alumni.Members.DTO;

public class AlumniRegistrationRequestDTO
{
    public string? DOERegistrationId { get; set; }
    public int Salutation { get; set; }
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DOB { get; set; }
    public int Gender { get; set; }
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }
    public string? BranchId { get; set; }
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
    public string? OtherProfession { get; set; }
    public bool IsResidentOfDelhi { get; set; }
    public string Password { get; set; } = default!;
}
