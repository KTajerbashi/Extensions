﻿using Dapper;
using Extensions.ChangeDataLog.Sql.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using Extensions.ChangeDataLog.Abstractions;

namespace Extensions.ChangeDataLog.Sql;

/// <summary>
/// پیاده سازی انترسیپتور برای 
/// SQL SERVER
/// و ذخیره لاگ تغییرات روی پایگاه داده
/// </summary>
public class DapperEntityChangeInterceptorItemRepository : IEntityChangeInterceptorItemRepository
{
    private readonly ChangeDataLogSqlOptions _options;
    private readonly IDbConnection _dbConnection;
    private string InsertEntityChangeInterceptorItemCommand = @"
    INSERT INTO [{0}].[{1}]
            ([Id],[ContextName],[EntityType],[EntityId],[UserId],[IP],[TransactionId],[DateOfOccurrence],[ChangeType]) 
    VALUES 
            (@Id,@ContextName,@EntityType,@EntityId,@UserId,@IP,@TransactionId,@DateOfOccurrence,@ChangeType) ";
   
    private string InsertPropertyChangeLogItemCommand = @"
    INSERT INTO [{0}].[{1}]
            ([Id],[ChangeInterceptorItemId],[PropertyName],[Value]) 
    VALUES
            (@Id,@ChangeInterceptorItemId,@PropertyName,@Value) ";

    public DapperEntityChangeInterceptorItemRepository(IOptions<ChangeDataLogSqlOptions> options)
    {
        _options = options.Value;
        _dbConnection = new SqlConnection(_options.ConnectionString);
        if (_options.AutoCreateSqlTable)
        {
            CreateEntityChangeInterceptorItemTableIfNeeded();
            CreatePropertyChangeLogItemTableIfNeeded();
        }

        InsertEntityChangeInterceptorItemCommand = string.Format(InsertEntityChangeInterceptorItemCommand, _options.SchemaName, _options.EntityTableName);
        InsertPropertyChangeLogItemCommand = string.Format(InsertPropertyChangeLogItemCommand, _options.SchemaName, _options.PropertyTableName);
    }

    public void Save(List<EntityChangeInterceptorItem> entityChangeInterceptorItems)
    {
        foreach (var item in entityChangeInterceptorItems)
        {
            //if (_dbConnection.State == ConnectionState.Closed)
            //    _dbConnection.Open();
            //using var tran = _dbConnection.BeginTransaction();
            try
            {
                _dbConnection.Execute(InsertEntityChangeInterceptorItemCommand, new { item.Id, item.ContextName, item.EntityType, item.EntityId, item.UserId, item.Ip, item.TransactionId, item.DateOfOccurrence, item.ChangeType });
                _dbConnection.Execute(InsertPropertyChangeLogItemCommand, item.PropertyChangeLogItems.ToArray());
                // tran.Commit();
            }
            catch (Exception ex)
            {

                // tran.Rollback();
            }
        }

    }

    public Task SaveAsync(List<EntityChangeInterceptorItem> entityChangeInterceptorItems)
    {
        throw new NotImplementedException();
    }

    private void CreateEntityChangeInterceptorItemTableIfNeeded()
    {
        string table = _options.EntityTableName;
        string schema = _options.SchemaName;
        string createTable = $@"
BEGIN
	IF (SCHEMA_ID('{schema}') IS NULL) 
		BEGIN
		    EXEC ('CREATE SCHEMA [{schema}]')
		END
END
BEGIN
	IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}' AND  TABLE_NAME = '{table}' )) 
	Begin
	    CREATE TABLE 
	        [{schema}].[{table}]
	        ( 
	            Id uniqueidentifier primary key, 
	            ContextName nvarchar(200)  not null, 
	            EntityType nvarchar(200)  not null, 
	            EntityId nvarchar(200)  not null, 
	            UserId nvarchar(200) , 
	            [IP] nvarchar(50),
	            TransactionId nvarchar(50) ,
	            DateOfOccurrence Datetime  not null ,
	            ChangeType nvarchar(50)
	        )
	End
END
";
        _dbConnection.Execute(createTable);
    }

    private void CreatePropertyChangeLogItemTableIfNeeded()
    {
        string parentTable = _options.EntityTableName;
        string table = _options.PropertyTableName;
        string schema = _options.SchemaName;
        string createTable = $@"
BEGIN
	IF (SCHEMA_ID('{schema}') IS NULL) 
		BEGIN
		    EXEC ('CREATE SCHEMA [{schema}]')
		END
END
BEGIN
    IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{schema}' AND  TABLE_NAME = '{table}' )) 
    Begin 
        CREATE TABLE [{schema}].[{table}]
        ( 
            Id uniqueidentifier primary key, 
            ChangeInterceptorItemId uniqueidentifier references [{schema}].[{parentTable}](Id),
            PropertyName nvarchar(200)  not null,
            Value nvarchar(max) 
        )
    End
END
";
        _dbConnection.Execute(createTable);
    }
}
