using Serializers.WebApi.Swagger;
using Extensions.Serializers.EPPlus;
using Serializers.WebApi.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerService();

builder.Services.AddEPPlusExcelSerializer();

//builder.Services.AddSingleton<ISendEmail, SendSingleton>();
//builder.Services.AddScoped<ISendEmail, SendScope>();
builder.Services.AddTransient<ISendEmail, SendTransient>();


builder.Services.AddSingleton<IIDSingleton>(new ID());
builder.Services.AddScoped<IIDScoped, ID>();
builder.Services.AddTransient<IIDTransient, ID>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwaggerService();

app.Run();
