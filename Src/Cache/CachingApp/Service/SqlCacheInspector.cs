//using CachingApp.Models;
//using CachingApp.Repository;
//using Extensions.Caching.Distributed.Sql;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Options;

//namespace CachingApp.Service;

//public class SqlCacheInspector : ICacheInspector
//{
//    private readonly DistributedSqlCacheOptions _options;

//    public SqlCacheInspector(IOptions<DistributedSqlCacheOptions> options)
//    {
//        _options = options.Value;
//    }

//    public List<CacheRecord> GetAll()
//    {
//        var list = new List<CacheRecord>();

//        using var con = new SqlConnection(_options.ConnectionString);
//        con.Open();

//        var sql = $@"
//            SELECT 
//                [Id], 
//                CONVERT(VARCHAR(MAX), [Value]) as Val,
//                [ExpiresAtTime],
//                [SlidingExpirationInSeconds],
//                [AbsoluteExpiration]
//            FROM [{_options.SchemaName}].[{_options.TableName}]
//            ORDER BY ExpiresAtTime DESC";

//        using var cmd = new SqlCommand(sql, con);
//        using var reader = cmd.ExecuteReader();

//        while (reader.Read())
//        {
//            list.Add(new CacheRecord
//            {
//                Key = reader.GetString(0),
//                Value = reader.GetString(1),
//                ExpiresAtTime = reader.GetDateTimeOffset(2),
//                SlidingExpirationInSeconds = reader.IsDBNull(3) ? null : reader.GetInt64(3),
//                AbsoluteExpiration = reader.IsDBNull(4) ? null : reader.GetDateTimeOffset(4)
//            });
//        }

//        return list;
//    }
//}
