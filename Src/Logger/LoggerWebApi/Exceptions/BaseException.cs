namespace LoggerWebApi.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; }
    public string UserMessage { get; }

    protected BaseException(string message, string userMessage, int statusCode) : base(message)
    {
        UserMessage = userMessage;
        StatusCode = statusCode;
    }
}
