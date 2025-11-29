using Extensions.Logger.Abstractions;
using Microsoft.Extensions.Logging;

namespace Extensions.Logger;


public class AppLoggerAdapter : IAppLogger
{
    private readonly ILogger _logger;

    public AppLoggerAdapter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("Application");
    }

    public void LogInformation(string message, int eventId = 0)
        => _logger.LogInformation(new EventId(eventId), message);

    public void LogWarning(string message, int eventId = 0)
        => _logger.LogWarning(new EventId(eventId), message);

    public void LogError(string message, Exception? exception = null, int eventId = 0)
        => _logger.LogError(new EventId(eventId), exception, message);

    public void LogDebug(string message, int eventId = 0)
        => _logger.LogDebug(new EventId(eventId), message);

    public void LogTrace(string message, int eventId = 0)
        => _logger.LogTrace(new EventId(eventId), message);
}

public class AppLoggerAdapter<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;

    public AppLoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message, int eventId = 0)
        => _logger.LogInformation(new EventId(eventId), message);

    public void LogWarning(string message, int eventId = 0)
        => _logger.LogWarning(new EventId(eventId), message);

    public void LogError(string message, Exception? exception = null, int eventId = 0)
        => _logger.LogError(new EventId(eventId), exception, message);

    public void LogDebug(string message, int eventId = 0)
        => _logger.LogDebug(new EventId(eventId), message);

    public void LogTrace(string message, int eventId = 0)
        => _logger.LogTrace(new EventId(eventId), message);
}


