using Serilog;
using System.Text;

namespace LoggerApp.Middlewares;


public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // --- REQUEST ---
        context.Request.EnableBuffering();
        string requestBody = "";
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        // Replace response stream
        var originalStream = context.Response.Body;
        using var newResponseStream = new MemoryStream();
        context.Response.Body = newResponseStream;

        await _next(context);

        // --- RESPONSE ---
        newResponseStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(newResponseStream).ReadToEndAsync();
        newResponseStream.Seek(0, SeekOrigin.Begin);

        // Final structured log
        Log.Information("HTTP REQUEST/RESPONSE {@Data}", new
        {
            Request = requestBody,
            Response = responseBody,
            Path = context.Request.Path,
            StatusCode = context.Response.StatusCode
        });

        await newResponseStream.CopyToAsync(originalStream);
    }
}


