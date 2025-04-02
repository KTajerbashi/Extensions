using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using UsersManagement.WebApi.DataContext;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Extensions;

public static class IdentityExtensions
{

    public static WebApplicationBuilder IdentityByExtensions(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        //Finally add identity
        // Configure Identity based on type
        var identityType = IdentityType.Cookie; // Change this as needed
        switch (identityType)
        {
            case IdentityType.Cookie:
                builder.Services.AddUsersManagementIdentityCookie<ApplicationDbContext, ApplicationUser, ApplicationRole, long>();
                break;
            case IdentityType.Session:
                builder.Services.AddUsersManagementIdentitySession<ApplicationDbContext, ApplicationUser, ApplicationRole, long>();
                break;
            case IdentityType.JWT:
                builder.Services.AddUsersManagementIdentityJWT<ApplicationDbContext, ApplicationUser, ApplicationRole, long>(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    secretKey: configuration["Jwt:Key"]);
                break;
            case IdentityType.JWE:
                builder.Services.AddUsersManagementIdentityJWE<ApplicationDbContext, ApplicationUser, ApplicationRole, long>(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    secretKey: configuration["Jwt:Key"]);
                break;
        }

        return builder;
    }

    public static IServiceCollection AddUsersManagementIdentityService<TDbContext, TUser, TRole, TId>(this IServiceCollection services, IdentityType type)
        where TDbContext : DbContext
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {

        switch (type)
        {
            case IdentityType.Cookie:
                services.AddUsersManagementIdentityCookie<TDbContext, TUser, TRole, TId>();
                break;
            case IdentityType.Session:
                break;
            case IdentityType.JWT:
                break;
            case IdentityType.JWE:
                break;
            default:
                break;
        }

        services.AddIdentityServices<TUser, TRole, TId>();
        return services;
    }
    public static IServiceCollection AddIdentityServices<TUser, TRole, TId>(this IServiceCollection services)
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {
        services.AddScoped<RoleManager<TRole>>();

        return services;
    }



    public static IServiceCollection AddUsersManagementIdentityCookie<TDbContext, TUser, TRole, TId>(
        this IServiceCollection services,
        Action<IdentityOptions>? configureIdentity = null,
        Action<CookieAuthenticationOptions>? configureCookie = null)
        where TDbContext : DbContext
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {
        // Add Identity with default options
        var identityBuilder = services.AddIdentity<TUser, TRole>(options =>
        {
            configureIdentity?.Invoke(options);
            
            // Default options if not configured
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        });

        // Add EntityFramework stores
        identityBuilder.AddEntityFrameworkStores<TDbContext>();
        identityBuilder.AddDefaultTokenProviders();

        // Configure application cookie
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "UsersManagement";
            //options.Cookie.Domain = "UsersManagement";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;

            configureCookie?.Invoke(options);
        });

        return services;
    }

    public static IServiceCollection AddUsersManagementIdentitySession<TDbContext, TUser, TRole, TId>(
        this IServiceCollection services,
        Action<IdentityOptions>? configureIdentity = null,
        Action<SessionOptions>? configureSession = null)
        where TDbContext : DbContext
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {
        // Add Identity with default options
        var identityBuilder = services.AddIdentity<TUser, TRole>(options =>
        {
            configureIdentity?.Invoke(options);
            
            // Default options if not configured
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        });

        // Add EntityFramework stores
        identityBuilder.AddEntityFrameworkStores<TDbContext>();
        identityBuilder.AddDefaultTokenProviders();

        // Add distributed memory cache and session
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(30);

            configureSession?.Invoke(options);
        });

        return services;
    }

    public static IServiceCollection AddUsersManagementIdentityJWT<TDbContext, TUser, TRole, TId>(
        this IServiceCollection services,
        string issuer,
        string audience,
        string secretKey,
        Action<IdentityOptions>? configureIdentity = null,
        Action<JwtBearerOptions>? configureJwt = null)
        where TDbContext : DbContext
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {
        // Add Identity with default options
        var identityBuilder = services.AddIdentity<TUser, TRole>(options =>
        {
            configureIdentity?.Invoke(options);
            
            // Default options if not configured
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        });

        // Add EntityFramework stores
        identityBuilder.AddEntityFrameworkStores<TDbContext>();
        identityBuilder.AddDefaultTokenProviders();

        // Add JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };

            configureJwt?.Invoke(options);
        });

        return services;
    }

    public static IServiceCollection AddUsersManagementIdentityJWE<TDbContext, TUser, TRole, TId>(
        this IServiceCollection services,
        string issuer,
        string audience,
        string secretKey,
        Action<IdentityOptions>? configureIdentity = null,
        Action<JwtBearerOptions>? configureJwt = null)
        where TDbContext : DbContext
        where TUser : IdentityUser<TId>
        where TRole : IdentityRole<TId>
        where TId : struct, IEquatable<TId>
    {
        // Add Identity with default options
        var identityBuilder = services.AddIdentity<TUser, TRole>(options =>
        {
            configureIdentity?.Invoke(options);
            
            // Default options if not configured
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
        });

        // Add EntityFramework stores
        identityBuilder.AddEntityFrameworkStores<TDbContext>();
        identityBuilder.AddDefaultTokenProviders();

        // Generate encryption key from secret
        using var sha256 = SHA256.Create();
        var encryptionKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));

        // Add JWT Authentication with JWE
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
            };

            configureJwt?.Invoke(options);
        });

        return services;
    }
}


