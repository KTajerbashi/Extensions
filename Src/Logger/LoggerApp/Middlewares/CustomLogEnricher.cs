using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace LoggerApp.Middlewares;

public class CustomLogEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _http;

    public CustomLogEnricher(IHttpContextAccessor http)
    {
        _http = http;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var ctx = _http.HttpContext;

        if (ctx == null)
            return;


        // TraceId
        var traceId = ctx.TraceIdentifier;
        logEvent.AddPropertyIfAbsent(factory.CreateProperty("TraceId", traceId));

        // User Agent
        var ua = ctx.Request.Headers["User-Agent"].ToString();
        logEvent.AddPropertyIfAbsent(factory.CreateProperty("UserAgent", ua));

        // IP
        var ip = ctx.Connection.RemoteIpAddress?.ToString();
        logEvent.AddPropertyIfAbsent(factory.CreateProperty("UserIp", ip));

        // User ID
        var userId = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        logEvent.AddPropertyIfAbsent(factory.CreateProperty("UserId", userId ?? "Anonymous"));

        // Controller / Action (only for MVC/API)
        var endpoint = ctx.GetEndpoint();
        var descriptor = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();

        if (descriptor != null)
        {
            logEvent.AddPropertyIfAbsent(factory.CreateProperty("Controller", descriptor.ControllerName));
            logEvent.AddPropertyIfAbsent(factory.CreateProperty("Action", descriptor.ActionName));
        }
    }
}

