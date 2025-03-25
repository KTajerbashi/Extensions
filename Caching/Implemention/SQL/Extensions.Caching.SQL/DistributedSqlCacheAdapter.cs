using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Extensions.Caching.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Extensions.Caching.SQL;

public class DistributedSqlCacheAdapter : ICacheAdapter
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedSqlCacheAdapter> _logger;

    public DistributedSqlCacheAdapter(IDistributedCache cache,ILogger<DistributedSqlCacheAdapter> logger)
    {
        _cache = cache;
        _logger = logger;

        _logger.LogInformation("DistributedCache Sql Adapter Start working");
    }

    public void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _logger.LogTrace("DistributedCache Sql Adapter Cache {obj} with key : {key} " +
                         ", with data : {data} " +
                         ", with absoluteExpiration : {absoluteExpiration} " +
                         ", with slidingExpiration : {slidingExpiration}",
                         typeof(TInput),
                         key,
                         JsonConvert.SerializeObject(obj),
                         absoluteExpiration.ToString(),
                         slidingExpiration.ToString());

        var option = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        };

        _cache.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj)), option);
    }

    public TOutput Get<TOutput>(string key)
    {
        var result = _cache.GetString(key);

        if (!string.IsNullOrWhiteSpace(result))
        {
            _logger.LogTrace("DistributedCache Sql Adapter Successful Get Cache with key : {key} and data : {data}",
                             key,
                             JsonConvert.SerializeObject(result));

            return JsonConvert.DeserializeObject<TOutput>(result);
        }
        else
        {
            _logger.LogTrace("DistributedCache Sql Adapter Failed Get Cache with key : {key}", key);

            return default;
        }
    }

    public void RemoveCache(string key)
    {
        _logger.LogTrace("DistributedCache Sql Adapter Remove Cache with key : {key}", key);

        _cache.Remove(key);
    }
}
