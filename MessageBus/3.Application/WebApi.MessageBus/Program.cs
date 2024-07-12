using Extensions.Serializers.NewtonSoft.Extensions.DependencyInjection;
using Extensions.MessageBus.RabbitMQ.Extensions.DependencyInjection;
using Extensions.MessageBus.MessageInbox.Extensions.DependencyInjection;
using Extensions.MessageBus.MessageInbox.Dal.Dapper.Extensions.DependencyInjection;
using MediateR.Extensions.Configurations.DependencyInjection;


#region MessageInbox
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSNewtonSoftSerializer();
builder.Services.AddDependenciesMediateR("WebApi.MessageBus");
builder.Services.AddRabbitMqMessageBus(c =>
{
    c.PerssistMessage = true;
    c.ExchangeName = "WebApi";
    c.ServiceName = "MessageBus";
    c.Url = @"amqp://guest:guest@localhost:5672/";
});
builder.Services.AddMessageInbox(c =>
{
    c.ApplicationName = "MessageBus";
    //c.ConnectionString = "Server=.;Initial Catalog=InboxDb;User Id=sa; Password=1qaz!QAZ;Encrypt=false";
});
builder.Services.AddMessageInboxDalSql(c =>
{
    //c.TableName = "MessageInbox";
    c.SchemaName = "dbo";
    c.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

#region RabbitMQ
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

#endregion
