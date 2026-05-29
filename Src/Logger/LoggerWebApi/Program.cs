using LoggerWebApi.Middlewares.ExceptionHandler;
using Serilog;

namespace LoggerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("")
                .CreateLogger();

            try
            {
                Log.Information("Starting Web API...");

                var builder = WebApplication.CreateBuilder(args);

                // Use Serilog
                builder.Host.UseSerilog();

                // Add services
                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Middleware
                app.UseExceptionMiddleware();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}