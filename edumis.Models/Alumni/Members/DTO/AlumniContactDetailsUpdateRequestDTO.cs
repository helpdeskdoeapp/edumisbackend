namespace edumis.Models.Alumni.Members.DTO;

public class AlumniContactDetailsUpdateRequestDTO
{
    //public string EmailID { get; set; } = default!;
    public string? AlternateEmailId { get; set; }
    public string? ResidenceContactNo { get; set; }   
    public string? MobileNo { get; set; }
    public bool IsResidentOfDelhi { get; set; }
    public string? CurrentResidence { get; set; }
    public string? CurrentResidenceCity { get; set; }    
}
