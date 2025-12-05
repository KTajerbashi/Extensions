namespace Extensions.Caching.Distributed.Sql;

/// <summary>
/// Represents configuration options for SQL distributed caching.
/// </summary>
public class DistributedSqlCacheOptions
{
    public bool AutoCreateTable { get; set; } = true;
    public string ConnectionString { get; set; } = string.Empty;
    public string SchemaName { get; set; } = "Cache";
    public string TableName { get; set; } = "Caching";
}
