using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SampleSource.Extensions;
using SampleSource.Providers.AutoFacDI;
using SampleSource.Providers.FluentEmails;

var builder = WebApplication.CreateBuilder(args);
//  Get Assemblies Of By Namespaces
var assemblies = ("SampleSource").GetAssemblies().ToArray();
builder.Services.AddControllers();

builder.Services.AddOpenApi();
// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample API's", Version = "v1" });
});
// Tell ASP.NET Core to use Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// Configure Autofac container
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.AddAutofacLifetimeServices(assemblies);
});

builder.Services.AddScoped<EmailService>();
builder.AddFluenEmails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API's V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetService<EmailService>();
    await service.RunAsync();
}

app.Run();