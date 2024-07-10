
using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Sql.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi.ChangeDataLog.DependencyInjections;
using Extensions.UsersManagement.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddChangeDatalogDalSql(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
});
builder.Services.AddHamsterChangeDataLog(config =>
{
    config.BusinessIdFieldName = "Id";
});
builder.Services.AddWebUserInfoService(config =>
{
    config.DefaultUserId = "1";
});

builder.Services.AddDependencies(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
