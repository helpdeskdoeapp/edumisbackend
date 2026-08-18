namespace edumisbackend.Common;

public class ResponseModel<T>
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; } = 200;

    public static ResponseModel<T> Success(T data, string message = "Success", int statusCode = 200)
        => new ResponseModel<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };

    public static ResponseModel<T> Failure(string message, int statusCode = 400) =>
        new ResponseModel<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode
        };
    
    public static ResponseModel<T> Unauthorized(string message = "Unauthorized", int statusCode = 401) =>
        new ResponseModel<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode
        };

    public static ResponseModel<T> NoData(string message = "No Data", int statusCode = 404) =>
       new ResponseModel<T>
       {
           IsSuccess = true,
           Message = message,
           StatusCode = statusCode
       };

    public static ResponseModel<T> ServerError(string message, int statusCode = 500) =>
       new ResponseModel<T>
       {
           IsSuccess = false,
           Message = message,
           StatusCode = statusCode          
       };
}
