

#region DI Builder
using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Hamster.Interceptors;
using Extensions.ChangeDataLog.Sql.Extensions.DependencyInjection;
using Extensions.UsersManagement.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
builder.Services.AddControllers();


builder.Services.AddChangeDatalogDalSql(option =>
{
    option.ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
});

builder.Services.AddHamsterChangeDataLog(option =>
{
    option.BusinessIdFieldName = "Id";
});

builder.Services.AddWebUserInfoService(option =>
{
    option.DefaultUserId = "1";
});

builder.Services.AddDbContext<DatabaseContext>(config =>
{
    config.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"))
    .AddInterceptors(new AddChangeDataLogInterceptor());
});
builder.Services.AddServiceWebApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion


#region APP
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
