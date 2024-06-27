

#region DI Builder
using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Hamster.Interceptors;
using Extensions.ChangeDataLog.Sql.Extensions.DependencyInjection;
using Extensions.UsersManagement.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.DependencyInjections;
using Extensions.Caching.InMemory.Extensions.DependencyInjection;
using Extensions.Caching.Distributed.Redis.Extensions.DependencyInjection;
using Extensions.Serializers.Abstractions;
using Extensions.Serializers.NewtonSoft.Services;
using Extensions.Caching.Distributed.Sql.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
builder.Services.AddControllers();


#region Change Data Log
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
#endregion

#region Caching
//builder.Services.AddKernelInMemoryCaching();
//builder.Services.AddKernelRedisDistributedCache(option =>
//{
//    option.Configuration = "localhost:9191,password=123456";
//    option.InstanceName = "WebApplicationAPI.";
//});
builder.Services.AddKernelSqlDistributedCache(builder.Configuration,"Cache");
#endregion
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
