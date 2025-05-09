﻿using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Extensions.Caching.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Extensions.Caching.SQL;

public static class DependencyInjections
{
    public static IServiceCollection AddSqlDistributedCache(this IServiceCollection services,
                                                                  IConfiguration configuration,
                                                                  string sectionName)
        => services.AddSqlDistributedCache(configuration.GetSection(sectionName));

    public static IServiceCollection AddSqlDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICacheAdapter, DistributedSqlCacheAdapter>();
        services.Configure<DistributedSqlCacheOptions>(configuration);

        var option = configuration.Get<DistributedSqlCacheOptions>();

        if (option.AutoCreateTable)
            CreateTable(option);

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = option.ConnectionString;
            options.SchemaName = option.SchemaName;
            options.TableName = option.TableName;
        });

        return services;
    }

    public static IServiceCollection AddSqlDistributedCache(this IServiceCollection services,
                                                            Action<DistributedSqlCacheOptions> setupAction)
    {
        services.AddTransient<ICacheAdapter, DistributedSqlCacheAdapter>();
        services.Configure(setupAction);

        var option = new DistributedSqlCacheOptions();
        setupAction.Invoke(option);

        if (option.AutoCreateTable)
            CreateTable(option);

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = option.ConnectionString;
            options.SchemaName = option.SchemaName;
            options.TableName = option.TableName;
        });

        return services;
    }

    private static void CreateTable(DistributedSqlCacheOptions options)
    {
        string table = options.TableName;
        string schema = options.SchemaName;

        string createTable =
            $"IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}' AND  TABLE_NAME = '{table}' ))" + "\n" +
            $"Begin" + "\n" +
            $"CREATE TABLE [{schema}].[{table}](" + "\n" +
            $"[Id][nvarchar](449) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL," + "\n" +
            $"[Value] [varbinary](max)NOT NULL," + "\n" +
            $"[ExpiresAtTime] [datetimeoffset](7) NOT NULL," + "\n" +
            $"[SlidingExpirationInSeconds] [bigint] NULL," + "\n" +
            $"[AbsoluteExpiration] [datetimeoffset](7) NULL," + "\n" +
            $"PRIMARY KEY(Id)," + "\n" +
            $"INDEX Index_ExpiresAtTime NONCLUSTERED (ExpiresAtTime))" + "\n" +
            $"End";

        var dbConnection = new SqlConnection(options.ConnectionString);
        dbConnection.Execute(createTable);
    }
}
