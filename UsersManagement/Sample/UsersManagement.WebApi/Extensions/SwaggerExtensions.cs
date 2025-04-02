using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using UsersManagement.WebApi.ActionFilters;

namespace UsersManagement.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithIdentity(this IServiceCollection services, IConfiguration configuration, IdentityType type)
    {
        services.AddSwaggerGen(static c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "🔐 Users Management 🔑 API 🗝️",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Name = "Tajerbashi",
                    Email = "kamrantajerbashi@gmail.com",
                    Url = new Uri("https://accounts.google.com/b/0/AddMailService")
                },
                Description = "📌 User Management With \n🔒Identity \n⚙️Cookie-Base, \n🪪Session-Base, \n🛰️Token-Base",
                License = new OpenApiLicense()
                {
                    Name = "MCIT",
                    Url = new Uri("https://accounts.google.com/b/0/AddMailService")
                },
                TermsOfService = new Uri("https://accounts.google.com/b/0/AddMailService"),
            });

            // Include XML comments if available
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);

            // Operation filter for all identity types
            c.OperationFilter<SecurityRequirementsOperationFilter>();

            // Add Bearer JWT Token Configuration
            ApiKey(c);
            //Http(c);
            //OAuth2(c);
            //OpenIdConnect(c);

        });

        return services;
    }

    private static void ApiKey(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.Example: Authorization: Bearer {token}",
            Name = "Authorization",
            Scheme = "Bearer",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.ApiKey,
            //BearerFormat = "Bearer <Token>"
        });
    }
    private static void OpenIdConnect(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
            Name = "Authorization",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.OpenIdConnect,
            Scheme = "Bearer"
        });
    }
    private static void Http(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
            Name = "Authorization",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });
    }
    private static void OAuth2(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
            Name = "Authorization",
            In = ParameterLocation.Cookie,
            Type = SecuritySchemeType.OAuth2,
            Scheme = "Bearer"
        });
    }

    public static WebApplication UseSwaggerWithIdentity(this WebApplication app, IdentityType type)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            if (type == IdentityType.JWT)
            {
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
            else
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users Management API v1");
                    c.ExposeSwaggerDocumentUrlsRoute = true;
                });
            }

        }
        return app;
    }
}








