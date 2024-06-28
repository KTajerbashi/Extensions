

#region DI Builder
using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Hamster.Interceptors;
using Extensions.ChangeDataLog.Sql.Extensions.DependencyInjection;
using Extensions.UsersManagement.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.DependencyInjections;
using Extensions.Caching.InMemory.Extensions.DependencyInjection;
using Extensions.Caching.Distributed.Redis.Extensions.DependencyInjection;
using Extensions.Serializers.Abstractions;
using Extensions.Serializers.NewtonSoft.Services;
using Extensions.Caching.Distributed.Sql.Extensions.DependencyInjection;
using Application.Layer.Services.Interfaces;
using Application.Layer.Services.Repositories;
using Application.Layer.DataAccess.ChangeDataLog;
using Extensions.MessageBus.MessageInbox.Extensions.DependencyInjection;
using Extensions.MessageBus.MessageInbox.Dal.Dapper.Extensions.DependencyInjection;
using Extensions.MessageBus.RabbitMQ.Extensions.DependencyInjection;

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

#region Services
//builder.Services.AddScoped < typeof(IBaseRepository<,,>), typeof(BaseRepository<,,>) >();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
#endregion

#region Caching
//builder.Services.AddKernelInMemoryCaching();
//builder.Services.AddKernelRedisDistributedCache(option =>
//{
//    option.Configuration = "localhost:9191,password=123456";
//    option.InstanceName = "WebApplicationAPI.";
//});
builder.Services.AddKernelSqlDistributedCache(builder.Configuration, "Cache");
#endregion

#region Message Inbox
builder.Services.AddRabbitMqMessageBus(c =>
{
    c.PerssistMessage = true;
    c.ExchangeName = "ExtensionsExchange";
    c.ServiceName = "ExtensionsService";
    c.Url = @"amqp://guest:guest@localhost:5672/";
});
builder.Services.AddMessageInbox(config =>
{
    config.ApplicationName = "WebApplicationAPI";
});
builder.Services.AddMessageInboxDalSql(config =>
{
    config.SchemaName = "Message";
    config.ConnectionString = configuration.GetSection("MessageInbox:ConnectionString").Value;
});
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
app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("MiniBlog", "BlogCreated"));
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
