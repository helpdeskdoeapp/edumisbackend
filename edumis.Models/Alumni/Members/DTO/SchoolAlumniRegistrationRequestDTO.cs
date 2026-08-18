namespace edumis.Models.Alumni.Members.DTO;

public class SchoolAlumniRegistrationRequestDTO
{
    public string BranchId { get; set; } = default!;
    public string? DOERegistrationId { get; set; }
    public int Salutation { get; set; }
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DOB { get; set; }
    public int Gender { get; set; }
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }   
    public string? CurrentOrganization { get; set; }
    public string? CurrentDesignation { get; set; }    
    public int? CurrentProfession { get; set; }
    public string? OtherProfession { get; set; }
    public bool IsResidentOfDelhi { get; set; }
    public bool CreateLogin { get; set; }
    public string? EmailID { get; set; }
    public string? MobileNo { get; set; }
}
