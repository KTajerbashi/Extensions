using Extensions.Caching.InMemory;
using Extensions.Caching.Redis;
using Extensions.Caching.SQL;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInMemoryCaching();
builder.Services.AddRedisDistributedCache((option) =>
{
    option.Configuration = "localhost:9191,password=123456";
    option.InstanceName = "Extensions.Caching.";
});
builder.Services.AddSqlDistributedCache((option) =>
{
    option.ConnectionString = builder.Configuration.GetConnectionString("");
    option.AutoCreateTable = true;
    option.TableName = "CacheData";
    option.SchemaName = "Cache";
    

});
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
