using Extensions.Logger;
using LoggerApp.Components;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// --------------------- Add services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddLogger();

// --------------------- Serilog Configuration

// Define custom SQL columns
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Add(StandardColumn.LogEvent); // Keep log event JSON
//columnOptions.Store.Add(StandardColumn.Message);  // Optional: message column
//columnOptions.Store.Add(StandardColumn.Exception);// Optional: exception column

columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "Controller", DataType = System.Data.SqlDbType.NVarChar, DataLength = 200 },
    new SqlColumn { ColumnName = "Action", DataType = System.Data.SqlDbType.NVarChar, DataLength = 200 },
    new SqlColumn { ColumnName = "UserId", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 }
};

// MSSQL sink options
var sinkOptions = new MSSqlServerSinkOptions
{
    TableName = "Logs",
    SchemaName = "Log",   // Custom schema
    AutoCreateSqlTable = true
};

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Read Console/File sinks and settings from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.MSSqlServer(
        connectionString: configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: sinkOptions,
        columnOptions: columnOptions
    )
    .CreateLogger();

// Use Serilog as logging provider
builder.Host.UseSerilog();

var app = builder.Build();

// --------------------- HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
