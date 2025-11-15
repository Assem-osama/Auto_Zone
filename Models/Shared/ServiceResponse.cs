public class ServiceResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ServiceResponse<T> SuccessResponse(T? data, string message = "Success") =>
        new() { Success = true, Data = data, Message = message };

    public static ServiceResponse<T> FailureResponse(string message) =>
        new() { Success = false, Message = message };
}
