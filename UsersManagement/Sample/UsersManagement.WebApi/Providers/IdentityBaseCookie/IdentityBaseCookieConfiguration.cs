using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UsersManagement.WebApi.DataContext;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Providers.IdentityBaseCookie;

public static class IdentityBaseCookieConfiguration
{
    public static IServiceCollection AddAuthenticationConfigurations(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Authentication:Cookies")
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddRoles<ApplicationRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            ;

        services
            .AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            // Add Identity.Application cookie (required by Identity)
            .AddCookie(options =>
            {
                ConfigureCookieOptions(options);
            })
            ;

        services.AddAuthorizationServices(configuration);

        return services;
    }

    private static void ConfigureCookieOptions(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = "AngularJs_WebApp";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    }

    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole",
                policy => policy.RequireRole("Admin"));

            options.AddPolicy("RequireUserRole",
                policy => policy.RequireRole("User"));

            options.AddPolicy("EditContent",
                policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(c =>
                        c.Type == "EditContent" && c.Value == "true") ||
                    context.User.IsInRole("Admin")));
        });

        return services;
    }

}

