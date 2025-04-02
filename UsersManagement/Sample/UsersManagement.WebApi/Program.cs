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

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerWithIdentity(configuration, IdentityType.Cookie);

// Add Identity Configuration
builder.Services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            ;

// Add Cookie Conffiguration
builder.Services.AddCookieConfigurations(configuration,"Cookie");

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityServices<ApplicationUser, ApplicationRole, long>();

// Register in Program.cs
builder.Services.AddHostedService<RefreshTokenCleanupService>();

// Add Repository
builder.Services.AddScoped<IIdentityRespository, IdentityRespository>();

var app = builder.Build();

app.UseSwaggerWithIdentity(IdentityType.Cookie);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add authentication before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRoutesAPIsRouter();

app.Run();

