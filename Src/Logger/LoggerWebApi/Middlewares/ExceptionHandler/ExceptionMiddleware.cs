using LoggerWebApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Diagnostics;

namespace LoggerWebApi.Middlewares.ExceptionHandler;

public static class ExceptionMiddleware
{
    private static (int statusCode, string userMessage, string message) MapException(Exception? exception)
    {
        return exception switch
        {
            DomainException ex => (ex.StatusCode, ex.UserMessage, ex.Message),
            AppException ex => (ex.StatusCode, ex.UserMessage, ex.Message),
            DataBaseException ex => (ex.StatusCode, ex.UserMessage, ex.Message),
            ApiException ex => (ex.StatusCode, ex.UserMessage, ex.Message),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "", "Access denied."),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "", "Resource not found."),
            _ => (StatusCodes.Status500InternalServerError, "", "An unexpected error occurred.")
        };
    }

    public static void UseExceptionMiddleware(this WebApplication app)
    {
        var processId = Process.GetCurrentProcess().Id;
        var isDevelopment = app.Environment.IsDevelopment();

        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = feature?.Error;

                var (statusCode, userMessage, message) = MapException(exception);
                string type = "Error";

                if (exception is BaseException baseException)
                {
                    statusCode = baseException.StatusCode;
                    message = $"{baseException.Message} | {baseException.UserMessage}";
                    type = exception.GetType().Name;
                }
                else if (exception is UnauthorizedAccessException)
                {
                    statusCode = StatusCodes.Status403Forbidden;
                    message = "Access denied.";
                    type = exception.GetType().Name;
                }
                else if (exception is KeyNotFoundException)
                {
                    statusCode = StatusCodes.Status404NotFound;
                    message = "Resource not found.";
                    type = exception.GetType().Name;
                }

                context.Response.StatusCode = statusCode;

                if (exception != null)
                {
                    Log.Error(
                        exception,
                        "Unhandled exception in {RequestPath} for {UserName} from {ClientIP}. ProcessId: {ProcessId}, StatusCode: {StatusCode}",
                        context.Request.Path,
                        context.User?.Identity?.Name ?? "Anonymous",
                        context.Connection.RemoteIpAddress?.ToString(),
                        processId,
                        statusCode
                    );
                }
                else
                {
                    Log.Error(
                        "Unhandled exception occurred but exception object was null. Path: {RequestPath}, ProcessId: {ProcessId}",
                        context.Request.Path,
                        processId
                    );
                }

                var errorResponse = new
                {
                    StatusCode = statusCode,
                    Message = message,
                    Type = isDevelopment ? exception?.GetType().Name : "Error",
                    StackTrace = isDevelopment ? exception?.StackTrace : null,
                    RequestPath = context.Request.Path.Value,
                    TraceId = Activity.Current?.Id ?? context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            });
        });
    }
}


