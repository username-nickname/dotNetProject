namespace Domain.Exceptions;

public class ExternalAuthException : Exception
{
    public int StatusCode { get; }

    public ExternalAuthException(string message, int statusCode = 401) : base(message)
    {
        StatusCode = statusCode;
    }
}