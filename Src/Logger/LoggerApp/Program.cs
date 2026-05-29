using Extensions.Logger;
using LoggerApp.Components;
using LoggerApp.Middlewares.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddLogger();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseExceptionMiddleware();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
