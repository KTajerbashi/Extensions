using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Services;

namespace UsersManagement.WebApi.Providers.IdentityBaseToken;

public static class Configuration
{
    public static IServiceCollection AddName(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        // JWT Configuration
        var jwtConfig = configuration.GetSection("Jwt");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtConfig["Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig["Secret"])),
                ClockSkew = TimeSpan.Zero // Remove tolerance for exact expiration
            };

            // For SignalR/WebSockets
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // JWT best practices
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdmin", policy =>
                policy.RequireRole("Admin"));

            // Fine-grained policy
            options.AddPolicy("EditContent", policy =>
                policy.RequireClaim("permission", "content.edit"));
        });

        // Register the token service
        services.AddScoped(typeof(TokenService<,>));

        // Register your IRefreshTokenStore implementation
        services.AddSingleton<IRefreshTokenStore, DatabaseRefreshTokenStore>(); // Or your preferred implementation

        // Refresh token store (simplified example)
        services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();
        return services;
    }
}
