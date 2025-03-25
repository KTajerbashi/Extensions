using Extensions.Caching.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Extensions.Caching.Redis;

public class DistributedRedisCacheAdapter : ICacheAdapter
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedRedisCacheAdapter> _logger;

    public DistributedRedisCacheAdapter(IDistributedCache cache,
                                        ILogger<DistributedRedisCacheAdapter> logger)
    {
        _cache = cache;
        _logger = logger;

        _logger.LogInformation("DistributedCache Redis Adapter Start working");
    }

    public void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _logger.LogTrace("DistributedCache Redis Adapter Cache {obj} with key : {key} " +
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
            _logger.LogTrace("DistributedCache Redis Adapter Successful Get Cache with key : {key} and data : {data}",
                             key,
                             JsonConvert.SerializeObject(result));

            return JsonConvert.DeserializeObject<TOutput>(result);
        }
        else
        {
            _logger.LogTrace("DistributedCache Redis Adapter Failed Get Cache with key : {key}", key);

            return default;
        }
    }

    public void RemoveCache(string key)
    {
        _logger.LogTrace("DistributedCache Redis Adapter Remove Cache with key : {key}", key);

        _cache.Remove(key);
    }
}
