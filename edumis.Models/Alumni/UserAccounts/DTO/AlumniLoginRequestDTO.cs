namespace edumis.Models.Alumni.UserAccounts.DTO;

public class AlumniLoginRequestDTO
{
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;

    //public string CaptchaToken { get; set; } = default!;

    public string? IPAddress { get; set; } = "0.0.0.0";

    public string? UserAgent { get; set; } = default!;
}

public class AlumniPasswordUpdateRequestDTO
{    
    public string OldPassword { get; set; } = default!;
    public string Password { get; set; } = default!;  
    public string? IPAddress { get; set; } = "0.0.0.0";
    public string? UserAgent { get; set; } = default!;
}

public class AlumniPasswordResetRequestDTO
{
    public string EmailId { get; set; } = default!;   
    public string Password { get; set; } = default!;
    public string? IPAddress { get; set; } = "0.0.0.0";
    public string? UserAgent { get; set; } = default!;
}

