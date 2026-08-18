namespace edumis.Models;

public class ErrorModel
{
    public int? ErrorCode { get; set; }

    public string Message { get; set; }

    public string? InnerExceptionMessage { get; set; }

    public string? StackTrace { get; set; }
}
