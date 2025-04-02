using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi;
using UsersManagement.WebApi.DataContext;
using UsersManagement.WebApi.Extensions;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.Entities;
using UsersManagement.WebApi.Providers.IdentityBaseCookie;
using UsersManagement.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerWithIdentity(configuration, IdentityType.Cookie);

// Anti-CSRF protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "CSRF-TOKEN";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add Scope Initializer
builder.Services.AddScoped<ApplicationDbContextSeedInitializer>();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

// Add Cookie Configuration
builder.Services.AddAuthenticationConfigurations(configuration);


//  Add Identity Service Injections
//builder.Services.AddIdentityServices<ApplicationUser, ApplicationRole, long>();

// Register in Program.cs
builder.Services.AddHostedService<RefreshTokenCleanupService>();

// Add Repository
builder.Services.AddScoped<IIdentityRespository, IdentityRespository>();

var app = builder.Build();

app.UseSwaggerWithIdentity(IdentityType.Cookie);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.InitialDatabaseAsync();

app.UseHttpsRedirection();

// Add authentication before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRoutesAPIsRouter();

app.Run();

