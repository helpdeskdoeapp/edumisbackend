using System.Text.Json.Serialization;

namespace edumis.Models.Users.DTO;

public class ReCaptchaResponseDTO
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; } = default!;      

    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    [JsonPropertyName("hostname")]
    public string Hostname { get; set; } = default!;

    [JsonPropertyName("error-codes")]
    public List<string> ErrorCodes { get; set; } = default!;
}
