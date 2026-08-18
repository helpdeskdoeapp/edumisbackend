namespace edumis.Models.Users.DTO;

public class RevokeTokenRequest
{
    public string RefreshToken { get; set; } = default!;
}
