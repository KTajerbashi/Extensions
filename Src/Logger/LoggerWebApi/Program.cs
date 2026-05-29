using LoggerWebApi.Contexts;
using LoggerWebApi.Middlewares.ExceptionHandler;
using LoggerWebApi.Providers;
using Serilog;

namespace LoggerWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            // Services

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IUserContext, UserContext>();

            builder.Services.AddScoped<RequestEnricher>();

            builder.Services.AddScoped<UserEnricher>();


            // Configure Serilog
            builder.CreateBuilderOnSerilog();


            var app = builder.Build();


            // Middlewares

            app.UseExceptionMiddleware();

            app.UseSerilogAddEnrichers();
            //app.UseSerilogRequestLogging();
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} " +
                    "responded {StatusCode} in {Elapsed:0.0000} ms";
            });

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
            Log.Fatal(ex,
                "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}