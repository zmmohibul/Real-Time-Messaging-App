using API.Errors;

namespace API.Helpers;

public class Result<T> 
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public Error? Error { get; set; }
    
    public static Result<T> OkResult(T data)
    {
        return new Result<T>()
        {
            Data = data,
            StatusCode = 200,
            IsSuccess = true
        };
    }
    
    public static Result<T> DataCreatedResult(T data)
    {
        return new Result<T>()
        {
            Data = data,
            StatusCode = 201,
            IsSuccess = true
        };
    }
    
    public static Result<T> UnauthorizedResult(string message)
    {
        return new Result<T>()
        {
            ErrorMessage = message,
            StatusCode = 401,
            IsSuccess = false,
            Error = new Error()
            {
                StatusCode = 401,
                ErrorMessage = message
            }
        };
    }
    
    public static Result<T> NotFoundResult(string message)
    {
        return new Result<T>()
        {
            StatusCode = 404,
            ErrorMessage = message,
            IsSuccess = false,
            Error = new Error()
            {
                StatusCode = 404,
                ErrorMessage = message
            }
        };
    }
    
    public static Result<T> BadRequestResult(string message)
    {
        return new Result<T>()
        {
            StatusCode = 400,
            ErrorMessage = message,
            IsSuccess = false,
            Error = new Error()
            {
                StatusCode = 400,
                ErrorMessage = message
            }
        };
    }
    
    public static Result<T> NoContentResult()
    {
        return new Result<T>()
        {
            StatusCode = 204,
            IsSuccess = true
        };
    }
}