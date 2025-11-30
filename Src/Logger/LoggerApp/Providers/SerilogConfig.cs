using LoggerApp.Middlewares;
using Serilog;

public static class SerilogConfig
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.With(new CustomLogEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()))
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
