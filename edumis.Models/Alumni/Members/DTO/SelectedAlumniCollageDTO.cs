namespace edumis.Models.Alumni.Members.DTO;

public class SelectedAlumniCollageDTO
{
    public string SalutationTitle { get; set; } = default!;
    public string Name { get; set; } = default!;
    public byte[]? ProfileImage { get; set; }
    public string? ProfileImageExtn { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? ImageUrl { get; set; } = default!;
    public string? BranchId { get; set; }
    public string? BranchName { get; set; }
    public int RegistrationYear { get; set; }
    public int ExitYear { get; set; }
}
