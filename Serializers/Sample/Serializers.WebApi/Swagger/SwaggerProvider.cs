using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Serializers.WebApi.Swagger;

public static class SwaggerProvider
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Serializers API", Version = "v1" });
        });
        return services;
    }
    public static WebApplication UseSwaggerService(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Serializers API v1");
            options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
        });
        return app;
    }
}
