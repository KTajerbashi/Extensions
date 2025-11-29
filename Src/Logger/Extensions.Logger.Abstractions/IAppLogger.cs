namespace Extensions.Logger.Abstractions;

public interface IAppLogger
{
    void LogInformation(string message, int eventId = 0);
    void LogWarning(string message, int eventId = 0);
    void LogError(string message, Exception? exception = null, int eventId = 0);
    void LogDebug(string message, int eventId = 0);
    void LogTrace(string message, int eventId = 0);
}

public interface IAppLogger<T> : IAppLogger
{
}
