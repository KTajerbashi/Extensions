using LoggerWebApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Diagnostics;

namespace LoggerWebApi.Middlewares.ExceptionHandler;

public static class ExceptionMiddleware
{
    public static void UseExceptionMiddleware(
        this WebApplication app)
    {
        var isDevelopment =
            app.Environment.IsDevelopment();

        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("ExceptionMiddleware");

                var feature = context.Features.Get<IExceptionHandlerFeature>();

                var exception = feature?.Error;

                var error = MapException(exception);

                context.Response.StatusCode = error.StatusCode;

                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                var correlationId = context.Items["CorrelationId"]?.ToString();

                var requestPath = context.Request.Path.Value;

                var requestMethod = context.Request.Method;

                var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                var userName = context.User?.Identity?.Name;

                var clientIp = context.Connection.RemoteIpAddress?.ToString();

                using (LogContext.PushProperty("TraceId", traceId))
                using (LogContext.PushProperty("CorrelationId", correlationId))
                using (LogContext.PushProperty("RequestPath", requestPath))
                using (LogContext.PushProperty("RequestMethod", requestMethod))
                using (LogContext.PushProperty("UserId", userId))
                using (LogContext.PushProperty("UserName", userName))
                using (LogContext.PushProperty("ClientIP", clientIp))
                using (LogContext.PushProperty("StatusCode", error.StatusCode))
                using (LogContext.PushProperty("ExceptionType", exception?.GetType().Name))
                using (LogContext.PushProperty("Exception", $"{exception?.Message} | {exception?.InnerException?.Message}"))
                {
                    logger.LogError(exception,
                        """
                        Unhandled Exception
                        StatusCode: {StatusCode}
                        Method: {RequestMethod}
                        Path: {RequestPath}
                        TraceId: {TraceId}
                        CorrelationId: {CorrelationId}
                        UserId: {UserId}
                        UserName: {UserName}
                        ClientIP: {ClientIP}
                        """,
                        error.StatusCode,
                        requestMethod,
                        requestPath,
                        traceId,
                        correlationId,
                        userId,
                        userName,
                        clientIp);
                }

                var problem = new ProblemDetails
                {
                    Status = error.StatusCode,
                    Title = error.Title,
                    Detail = isDevelopment ? error.DeveloperMessage : error.UserMessage,
                    Instance = requestPath,
                    Type = $"https://httpstatuses.com/{error.StatusCode}"
                };

                problem.Extensions["traceId"] = traceId;
                problem.Extensions["correlationId"] = correlationId;

                if (isDevelopment)
                {
                    problem.Extensions["stackTrace"] = exception?.StackTrace;
                    problem.Extensions["exception"] = exception?.GetType().Name;
                    problem.Extensions["innerException"] = exception?.InnerException?.Message;
                }

                await context.Response
                    .WriteAsJsonAsync(problem);
            });
        });
    }

    private static ErrorResult MapException(Exception? exception)
    {
        return exception switch
        {
            BaseException ex => new ErrorResult
            {
                StatusCode = ex.StatusCode,
                Title = ex.GetType().Name,
                UserMessage = ex.UserMessage,
                DeveloperMessage = ex.Message
            },

            UnauthorizedAccessException => new ErrorResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                UserMessage = "Access denied.",
                DeveloperMessage = exception.Message
            },

            KeyNotFoundException => new ErrorResult
            {
                StatusCode =
                    StatusCodes.Status404NotFound,

                Title = "Not Found",

                UserMessage =
                    "Resource not found.",

                DeveloperMessage =
                    exception.Message
            },

            _ => new ErrorResult
            {
                StatusCode =
                    StatusCodes.Status500InternalServerError,

                Title = "Internal Server Error",

                UserMessage =
                    "Internal server error.",

                DeveloperMessage =
                    exception?.Message
                    ?? "Unknown error"
            }
        };
    }

    private sealed class ErrorResult
    {
        public int StatusCode { get; set; }

        public string Title { get; set; }
            = default!;

        public string UserMessage { get; set; }
            = default!;

        public string DeveloperMessage { get; set; }
            = default!;
    }
}
