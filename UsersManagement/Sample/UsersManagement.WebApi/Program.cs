using UsersManagement.WebApi.DataContext;
using Extensions.UsersManagement.Configurations;
using UsersManagement.WebApi.Models.Entities;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using UsersManagement.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerWithIdentity(configuration,IdentityType.Cookie);

// Add Repository
builder.Services.AddScoped<IIdentityRespository, IdentityRespository>();

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

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

app.Run();
