using LoggerWebApi.Contexts;
using Serilog.Core;
using Serilog.Events;

namespace LoggerWebApi.Providers;


public class RequestEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _http;

    public RequestEnricher(
        IHttpContextAccessor http)
    {
        _http = http;
    }

    public void Enrich(
        LogEvent logEvent,
        ILogEventPropertyFactory factory)
    {
        var context = _http.HttpContext;

        if (context == null)
            return;

        logEvent.AddPropertyIfAbsent(
            factory.CreateProperty(
                "ClientIP",
                context.Connection
                    .RemoteIpAddress?
                    .ToString()));

        logEvent.AddPropertyIfAbsent(
            factory.CreateProperty(
                "RequestPath",
                context.Request.Path));

        logEvent.AddPropertyIfAbsent(
            factory.CreateProperty(
                "RequestMethod",
                context.Request.Method));

        logEvent.AddPropertyIfAbsent(
            factory.CreateProperty(
                "TraceId",
                context.TraceIdentifier));
    }
}

public class UserEnricher : ILogEventEnricher
{
    private readonly IUserContext _userContext;

    public UserEnricher(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public void Enrich(
        LogEvent logEvent,
        ILogEventPropertyFactory propertyFactory)
    {
        var userId =
            propertyFactory.CreateProperty(
                "UserId",
                _userContext.UserId);

        var userName =
            propertyFactory.CreateProperty(
                "UserName",
                _userContext.UserName);

        var clientIp =
            propertyFactory.CreateProperty(
                "ClientIP",
                _userContext.ClientIP);

        logEvent.AddPropertyIfAbsent(userId);
        logEvent.AddPropertyIfAbsent(userName);
        logEvent.AddPropertyIfAbsent(clientIp);
    }
}
