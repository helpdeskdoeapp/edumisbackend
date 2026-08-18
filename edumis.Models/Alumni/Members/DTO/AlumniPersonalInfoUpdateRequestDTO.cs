namespace edumis.Models.Alumni.Members.DTO;

public class AlumniPersonalInfoUpdateRequestDTO
{    
    public int Salutation { get; set; }
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DOB { get; set; }
    public int Gender { get; set; }
    public string EmailID { get; set; } = default!;
    public string? MobileNo { get; set; }
}