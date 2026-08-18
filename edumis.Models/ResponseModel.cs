namespace edumis.Models;

public class ResponseModel
{
    public string? ReturnId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? ReturnCode { get; set; }
}
