using static System.Runtime.InteropServices.JavaScript.JSType;

public class ServiceResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ServiceResponse<T> SuccessResponse(T? data, string message = "Success") =>
        new() { Success = true, Data = data, Message = message };

    public static ServiceResponse<T> FailureResponse(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}
