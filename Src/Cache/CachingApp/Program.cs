using CachingApp.Components;
//using CachingApp.Repository;
//using CachingApp.Service;
using Extensions.Caching.Distributed.Redis;
using Extensions.Serializers.Microsoft;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

#region InMemory Cache Config
//builder.Services.AddInMemoryCacheAdapter();
//builder.Services.AddMicrosoftSerializers();
#endregion

#region SQL Cache Config With Add appsetting.json => Cache : Section
//builder.Services.AddScoped<ICacheInspector, SqlCacheInspector>();
//builder.Services.AddSqlDistributedCache(configuration, "Cache");
//builder.Services.AddMicrosoftSerializers();
#endregion

#region Redis
builder.Services.AddRedisDistributedCache(option =>
{
    option.Configuration = "localhost:9191,password=123456";
    option.InstanceName = "CachingApp";
});
builder.Services.AddMicrosoftSerializers();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
