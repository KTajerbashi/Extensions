using Extensions.Caching.Distributed.Sql.Extensions.DependencyInjection;
using Extensions.Caching.Distributed.Redis.Extensions.DependencyInjection;
using Extensions.Caching.InMemory.Extensions.DependencyInjection;
using Extensions.Serializers.NewtonSoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSNewtonSoftSerializer();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region Redis Configuration
//builder.Services.AddKernelRedisDistributedCache(builder.Configuration, "RedisDistributedCache");
//builder.Services.AddKernelRedisDistributedCache(option =>
//{
//    option.Configuration = "localhost:9191,password=123456";
//    option.InstanceName = "WebApi.Caching";
//});
#endregion

#region InMemory Configuration
//builder.Services.AddKernelInMemoryCaching();
#endregion

#region Sql Server Configuration
builder.Services.AddKernelSqlDistributedCache(builder.Configuration, "SQLDistributedCache");
#endregion


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
