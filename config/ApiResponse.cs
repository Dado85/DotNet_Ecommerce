namespace Ecommerce.config;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors;
    public static ApiResponse<T> Succeed(T data, int statusCode = 200, string message = "Request Completed Successfully.")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            StatusCode = statusCode,
            Message = message

        };
    }
    public static ApiResponse<T> Failed(List<string>? errors = null, int statusCode = 400, string message = "Request Failed.")
    {
        return new ApiResponse<T>
        {
            Success = false,
            Errors = errors ?? [message],
            Message = message,
            StatusCode = statusCode
        };
    }
}