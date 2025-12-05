using Extensions.Caching.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Caching.Distributed.Sql;

/// <summary>
/// Extension methods for registering SQL distributed caching.
/// </summary>
public static class DependencyInjections
{
    public static IServiceCollection AddSqlDistributedCache(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        => services.AddSqlDistributedCache(configuration.GetSection(sectionName));

    public static IServiceCollection AddSqlDistributedCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<ICacheAdapter, CacheSqlAdapter>();
        services.Configure<DistributedSqlCacheOptions>(configuration);

        var options = configuration.Get<DistributedSqlCacheOptions>();
        ValidateOptions(options);

        if (options.AutoCreateTable)
            CreateCacheTable(options);

        RegisterSqlCache(services, options);

        return services;
    }

    public static IServiceCollection AddSqlDistributedCache(
        this IServiceCollection services,
        Action<DistributedSqlCacheOptions> setupAction)
    {
        services.AddTransient<ICacheAdapter, CacheSqlAdapter>();
        services.Configure(setupAction);

        var options = new DistributedSqlCacheOptions();
        setupAction(options);

        ValidateOptions(options);

        if (options.AutoCreateTable)
            CreateCacheTable(options);

        RegisterSqlCache(services, options);

        return services;
    }

    private static void ValidateOptions(DistributedSqlCacheOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            throw new ArgumentException("ConnectionString is required for SQL distributed caching.");

        if (string.IsNullOrWhiteSpace(options.TableName))
            throw new ArgumentException("TableName must be provided.");

        if (string.IsNullOrWhiteSpace(options.SchemaName))
            options.SchemaName = "dbo";
    }

    private static void RegisterSqlCache(IServiceCollection services, DistributedSqlCacheOptions options)
    {
        services.AddDistributedSqlServerCache(cacheOptions =>
        {
            cacheOptions.ConnectionString = options.ConnectionString;
            cacheOptions.SchemaName = options.SchemaName;
            cacheOptions.TableName = options.TableName;
        });
    }

    private static void CreateCacheTable(DistributedSqlCacheOptions options)
    {
        var sql = $@"
IF NOT EXISTS (
    SELECT * 
    FROM sys.schemas 
    WHERE name = @SchemaName
)
BEGIN
    EXEC('CREATE SCHEMA [' + @SchemaName + '] AUTHORIZATION dbo');
END

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = @SchemaName
      AND TABLE_NAME = @TableName
)
BEGIN
    EXEC('
        CREATE TABLE [' + @SchemaName + '].[' + @TableName + '](
            [Id] NVARCHAR(449) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
            [Value] VARBINARY(MAX) NOT NULL,
            [ExpiresAtTime] DATETIMEOFFSET(7) NOT NULL,
            [SlidingExpirationInSeconds] BIGINT NULL,
            [AbsoluteExpiration] DATETIMEOFFSET(7) NULL,
            CONSTRAINT PK_' + @TableName + ' PRIMARY KEY CLUSTERED ([Id]),
            INDEX IX_' + @TableName + '_ExpiresAtTime NONCLUSTERED([ExpiresAtTime])
        )
    ');
END
";

        using var connection = new SqlConnection(options.ConnectionString);
        using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@SchemaName", options.SchemaName);
        command.Parameters.AddWithValue("@TableName", options.TableName);

        connection.Open();
        command.ExecuteNonQuery();
    }

}