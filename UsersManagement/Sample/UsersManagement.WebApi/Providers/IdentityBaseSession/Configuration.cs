using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace UsersManagement.WebApi.Providers.IdentityBaseSession;

public static class Configuration
{
    public static IServiceCollection AddName(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {

        // Install Microsoft.Extensions.Caching.StackExchangeRedis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "SessionStore_";
        });

        services.AddSession(options =>
        {
            options.Cookie.Name = "App.Session";
            options.IdleTimeout = TimeSpan.FromHours(2);
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;

            // For high-security apps
            options.Cookie.SameSite = SameSiteMode.Strict;
        });

        return services;
    }


}



// Extension methods for complex objects
public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

// Usage
//HttpContext.Session.SetObject("UserPreferences", preferences);
//var prefs = HttpContext.Session.GetObject<UserPreferences>("UserPreferences");
