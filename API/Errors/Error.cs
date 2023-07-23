namespace API.Errors;

public class Error
{
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public Error()
    {
    }

    public Error(int statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}