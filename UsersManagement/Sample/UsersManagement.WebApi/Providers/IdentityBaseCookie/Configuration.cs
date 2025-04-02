using Microsoft.AspNetCore.Authentication.Cookies;

namespace UsersManagement.WebApi.Providers.IdentityBaseCookie;

public static class Configuration
{
    public static IServiceCollection AddCookieConfigurations(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                // Core settings
                options.Cookie.Name = "App.Auth";
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/Forbidden";
                options.ReturnUrlParameter = "returnUrl";

                // Expiration settings
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.SlidingExpiration = true;

                // Cookie security
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                //options.Cookie.Domain = "yourdomain.com";

                // Events
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.CompletedTask;
                    }
                };

                // Advanced cookie protection
                options.Cookie = new CookieBuilder
                {
                    Name = "SecureAuth",
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true,
                    SecurePolicy = CookieSecurePolicy.Always,
                    IsEssential = true,
                    MaxAge = TimeSpan.FromHours(8),
                    Domain = ".mydomain.com"
                };
            });

        // Policy best practices
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdmin", policy =>
                policy.RequireRole("Admin"));

            // Fine-grained policy
            options.AddPolicy("EditContent", policy =>
                policy.RequireClaim("permission", "content.edit"));
        });
        return services;
    }
}


