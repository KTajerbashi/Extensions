using LoggerWebApi.Exceptions;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace LoggerWebApi.Providers;

public static class SerilogExtensions
{
    public static WebApplicationBuilder CreateBuilderOnSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(
            (context, services, configuration) =>
            {
                configuration

                    .MinimumLevel.Warning()

                    .Enrich.FromLogContext()

                    .Enrich.WithProperty("ApplicationName", "LoggerWebApi")

                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProcessId()
                    .Enrich.WithEnvironmentName()


                    // Custom Enrichers
                    //.Enrich.With(services.GetRequiredService<RequestEnricher>())
                    //.Enrich.With(services.GetRequiredService<UserEnricher>())

                    // Console
                    .WriteTo.Async(x => x.Console())

                    // File
                    .WriteTo.Async(x => x.File(
                        path: "Logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 100 * 1024 * 1024,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 30,
                        shared: true,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))

                    // SQL Server
                    .WriteTo.Async(x => x.MSSqlServer(
                        connectionString: GetConnectionString(context.Configuration),
                        sinkOptions: GetSinkOptions(),
                        columnOptions: GetColumnOptions())
                    )
                    ;
            });

        return builder;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        return configuration
            .GetConnectionString("DefaultConnection")
            ?? throw new DataBaseException(
                "Connection String Not Found");
    }

    private static MSSqlServerSinkOptions GetSinkOptions()
    {
        return new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            SchemaName = "Log",
            AutoCreateSqlTable = true
        };
    }

    private static ColumnOptions GetColumnOptions()
    {
        var options = new ColumnOptions();

        options.Store.Add(StandardColumn.LogEvent);

        options.AdditionalColumns =
        [
            new SqlColumn("ApplicationName",SqlDbType.NVarChar,dataLength: 200),
            new SqlColumn("UserId", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("UserName", SqlDbType.NVarChar, dataLength: 256),
            new SqlColumn("ClientIP", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("RequestPath", SqlDbType.NVarChar, dataLength: 500),
            new SqlColumn("RequestMethod", SqlDbType.NVarChar, dataLength: 20),
            new SqlColumn("MachineName", SqlDbType.NVarChar, dataLength: 200),
            new SqlColumn("EnvironmentName", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("TraceId", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("ThreadId",SqlDbType.Int),
            new SqlColumn("ProcessId",SqlDbType.Int),
            new SqlColumn("CorrelationId", SqlDbType.NVarChar, dataLength: 100),
            new SqlColumn("SourceContext", SqlDbType.NVarChar, dataLength: 500),
            new SqlColumn("UserAgent", SqlDbType.NVarChar, dataLength: 1000),
            new SqlColumn("StatusCode", SqlDbType.Int),
            new SqlColumn("ExceptionType", SqlDbType.VarChar,dataLength:100),
            new SqlColumn("ElapsedMilliseconds", SqlDbType.BigInt)
        ];

        return options;
    }

    public static IApplicationBuilder UseSerilogAddEnrichers(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var stopwatch = Stopwatch.StartNew();

            // User
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            var userName = context.User?.Identity?.Name ?? "Anonymous";

            // CorrelationId
            const string headerName = "X-Correlation-Id";
            var correlationId = context.Request.Headers[headerName].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Response.Headers[headerName] = correlationId;
            context.Items["CorrelationId"] = correlationId;

            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("ClientIP", context.Connection.RemoteIpAddress?.ToString()))
            using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("UserAgent", context.Request.Headers.UserAgent.ToString()))
            {
                try
                {
                    await next();
                }
                finally
                {
                    stopwatch.Stop();

                    var elapsed = stopwatch.ElapsedMilliseconds;
                    using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
                    using (LogContext.PushProperty("ElapsedMilliseconds", elapsed))
                    {
                        Log.Information(
                            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms",
                            context.Request.Method,
                            context.Request.Path,
                            context.Response.StatusCode,
                            elapsed);
                    }
                }
            }
        });

        return app;
    }
}
