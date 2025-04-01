using UsersManagement.WebApi.DataContext;
using Extensions.UsersManagement.Configurations;
using UsersManagement.WebApi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


//Finally add identity
builder.Services.AddUsersManagementIdentityCookie<ApplicationDbContext, ApplicationUser, ApplicationRole>();

builder.Services.AddUsersManagementIdentitySession<ApplicationDbContext, ApplicationUser, ApplicationRole>();

builder.Services.AddUsersManagementIdentityJWT<ApplicationDbContext, ApplicationUser, ApplicationRole>(
    issuer: configuration["Jwt:Issuer"],
    audience: configuration["Jwt:Audience"],
    secretKey: configuration["Jwt:Key"]);

builder.Services.AddUsersManagementIdentityJWE<ApplicationDbContext, ApplicationUser, ApplicationRole>(
    issuer: configuration["Jwt:Issuer"],
    audience: configuration["Jwt:Audience"],
    secretKey: configuration["Jwt:Key"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
