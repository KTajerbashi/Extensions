using Extensions.MessageBus.MessageInbox.Dal.Dapper.Extensions.DependencyInjection;
using Extensions.MessageBus.MessageInbox.Extensions.DependencyInjection;
using Extensions.MessageBus.RabbitMQ.Extensions.DependencyInjection;
using Extensions.Serializers.NewtonSoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNewtonSoftSerializer();
builder.Services.AddRabbitMqMessageBus(c =>
{
    c.PerssistMessage = true;
    c.ExchangeName = "WebApplicationAPI";
    c.ServiceName = "WebApplicationAPI.RabbitMQ";
    c.Url = @"amqp://guest:guest@localhost:5672/";
});
builder.Services.AddMessageInbox(c =>
{
    c.ApplicationName = "WebApplicationAPI.RabbitMQ";
});
builder.Services.AddMessageInboxDalSql(c =>
{
    //c.TableName = "MessageInbox";
    c.SchemaName = "Message";
    c.ConnectionString = configuration.GetSection("MessageInbox:ConnectionString").Value;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("WebApplicationAPI_RabbitMQ", "WebApplicationAPI_RabbitMQ_Created"));

app.UseAuthorization();

app.MapControllers();

app.Run();
