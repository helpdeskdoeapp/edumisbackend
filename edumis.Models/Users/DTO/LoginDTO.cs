namespace edumis.Models.Users.DTO;

public class LoginDTO
{
    public required string UserName { get; set; }

    public required string Password { get; set; }
    //public string CaptchaToken { get; set; } = default!;

    public string? IPAddress { get; set; } = "0.0.0.0";

    public string? UserAgent { get; set; } = "app";

    // public required string Salt { get; set; }
}
