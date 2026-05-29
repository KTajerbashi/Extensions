using Extensions.Logger;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

public static class DependencyInjection
{
    static async Task EnsureDatabaseCreatedAsync(string connStr, string dbName)
    {
        await using var conn = new SqlConnection(connStr);
        await conn.OpenAsync();

        var existsCmd = new SqlCommand(
            "SELECT COUNT(*) FROM sys.databases WHERE name = @dbName", conn);
        existsCmd.Parameters.AddWithValue("@dbName", dbName);

        var count = (int)await existsCmd.ExecuteScalarAsync();
        if (count > 0) return;

        var createCmd = new SqlCommand(
            $"CREATE DATABASE [{dbName}];", conn);
        await createCmd.ExecuteNonQueryAsync();
    }

    // --- NEW METHOD: Ensure Table is Created ---
    static async Task EnsureTableCreatedAsync(string connStrDb, string tableName, string schemaName)
    {
        await using var conn = new SqlConnection(connStrDb);
        await conn.OpenAsync();
        // Check if the table exists
        var tableExistsCmd = new SqlCommand(
            @"SELECT COUNT(*)
              FROM INFORMATION_SCHEMA.TABLES
              WHERE TABLE_SCHEMA = @SchemaName AND TABLE_NAME = @TableName", conn);
        tableExistsCmd.Parameters.AddWithValue("@SchemaName", schemaName);
        tableExistsCmd.Parameters.AddWithValue("@TableName", tableName);
        var tableCount = (int)await tableExistsCmd.ExecuteScalarAsync();
        if (tableCount > 0)
        {
            Console.WriteLine($"Table '{schemaName}.{tableName}' already exists.");
            return;
        }
        Console.WriteLine($"Creating table '{schemaName}.{tableName}'...");
        // Define your CREATE TABLE statement here.
        // Make sure the data types and column names match your Serilog configuration
        // and what you intend to log.
        var createTableSql = $@"
        CREATE TABLE [{schemaName}].[{tableName}]
        (
            Id INT IDENTITY(1,1) PRIMARY KEY,
            TimeStamp DATETIME2 NOT NULL,
            Level NVARCHAR(50) NULL,
            Message NVARCHAR(MAX) NULL,
            Exception NVARCHAR(MAX) NULL,
            SourceContext NVARCHAR(512) NULL,
            CorrelationId NVARCHAR(256) NULL, -- Example: If you log correlation ID
            RequestPath NVARCHAR(512) NULL,
            RequestMethod NVARCHAR(32) NULL,
            StatusCode INT NULL,
            ElapsedTime FLOAT NULL, -- Using FLOAT for elapsed time
            UserName NVARCHAR(128) NULL,
            ClientIP NVARCHAR(64) NULL,
            MachineName NVARCHAR(256) NULL,
            Environment NVARCHAR(100) NULL,
            Application NVARCHAR(256) NULL,
            ThreadId INT NULL,
            ProcessId INT NULL -- Example: If you log process ID
        );";


        var createSchemaCmd = new SqlCommand($"CREATE SCHEMA {schemaName};", conn);
        var createTableCmd = new SqlCommand(createTableSql, conn);

        await createSchemaCmd.ExecuteNonQueryAsync();
        await createTableCmd.ExecuteNonQueryAsync();

        Console.WriteLine($"Table '{schemaName}.{tableName}' created successfully.");
    }

    public static async Task<IServiceCollection> AddLoggerApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogger();

        var masterConnection = configuration.GetConnectionString("master");
        var defaultConnection = configuration.GetConnectionString("default");
        if (!masterConnection.IsNullOrEmpty())
        {
            //// --- Bootstrap Database and Table Creation ---
            //// 1. Ensure Database exists
            await EnsureDatabaseCreatedAsync(masterConnection!, "LoggerDB");
            //// 2. Ensure Table exists (using the LogDb connection string after the DB is confirmed to exist)
            await EnsureTableCreatedAsync(masterConnection!, "Logs", "Log");
        }

        services.AddHttpContextAccessor();
        services.AddRazorComponents().AddInteractiveServerComponents();


       

        return services;
    }

    public static WebApplication UseLoggerApp(this WebApplication app)
    {
        return app;
    }
}
