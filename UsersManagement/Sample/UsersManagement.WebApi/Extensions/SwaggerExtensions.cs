using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using UsersManagement.WebApi.ActionFilters;

namespace UsersManagement.WebApi.Extensions;

public class SwaggerOption
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
}


public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithIdentity(this IServiceCollection services, string identityType = "JWT")
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Include XML comments if available
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Add security definition based on identity type
            switch (identityType.ToUpper())
            {
                case "JWT":
                    AddJwtSecurityDefinition(c);
                    break;
                case "JWE":
                    AddJweSecurityDefinition(c);
                    break;
                case "COOKIE":
                    AddCookieSecurityDefinition(c);
                    break;
                case "SESSION":
                    AddSessionSecurityDefinition(c);
                    break;
                default:
                    AddJwtSecurityDefinition(c);
                    break;
            }

            // Operation filter for all identity types
            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        return services;
    }

    private static void AddJwtSecurityDefinition(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    }

    public static IServiceCollection AddJwtSecurityDefinition(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Users Management API", Version = "v1" });

            // Add security definition if using JWT
            if (configuration["Authentication:Type"]?.ToUpper() == "JWT" ||
                configuration["Authentication:Type"]?.ToUpper() == "JWE")
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Add this to your SwaggerGen configuration
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            }
        });
        return services;
    }

    private static void AddJweSecurityDefinition(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWE Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    }

    private static void AddCookieSecurityDefinition(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("CookieAuth", new OpenApiSecurityScheme
        {
            Description = "Cookie-based authentication",
            Name = ".AspNetCore.Identity.Application",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.ApiKey
        });
    }

    private static void AddSessionSecurityDefinition(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("SessionAuth", new OpenApiSecurityScheme
        {
            Description = "Session-based authentication",
            Name = ".AspNetCore.Session",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.ApiKey
        });
    }

    public static WebApplication UseSwaggerWithIdentity(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users Management API v1");

                // Enable OAuth for JWT if needed
                if (app.Configuration["Authentication:Type"]?.ToUpper() == "JWT" ||
                    app.Configuration["Authentication:Type"]?.ToUpper() == "JWE")
                {
                    c.OAuthClientId("swagger-ui");
                    c.OAuthAppName("Swagger UI");
                    c.OAuthUsePkce();
                }
            });
        }
        return app;
    }
}








