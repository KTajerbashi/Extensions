using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UsersManagement.WebApi.Providers.HybridApproach;

public static class Configuration
{
    public static IServiceCollection AddHybridApproach(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        // Support both cookie and JWT
        services.AddAuthentication()
            .AddCookie(options => { /* cookie config */ })
            .AddJwtBearer(options => { /* JWT config */ });

        // Then use policy scheme
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Hybrid";
            options.DefaultChallengeScheme = "Hybrid";
        })
        .AddPolicyScheme("Hybrid", "Hybrid", options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (authHeader?.StartsWith("Bearer ") == true)
                {
                    return JwtBearerDefaults.AuthenticationScheme;
                }
                return CookieAuthenticationDefaults.AuthenticationScheme;
            };
        });
        return services;
    }
}


