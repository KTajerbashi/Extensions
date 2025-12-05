using Extensions.Caching.Abstractions;
using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Extensions.Caching.Distributed.Sql;

/// <summary>
/// Distributed SQL cache adapter implementing <see cref="ICacheAdapter"/>.
/// Provides serialization, logging, and distributed cache operations.
/// </summary>
public class CacheSqlAdapter : ICacheAdapter
{
    private readonly IDistributedCache _cache;
    private readonly ISerializerJson _serializer;
    private readonly ILogger<CacheSqlAdapter> _logger;

    public CacheSqlAdapter(
        IDistributedCache cache,
        ISerializerJson serializer,
        ILogger<CacheSqlAdapter> logger)
    {
        _cache = cache;
        _serializer = serializer;
        _logger = logger;

        _logger.LogInformation("CacheSqlAdapter initialized and ready.");
    }

    /// <summary>
    /// Adds a value into the distributed cache.
    /// </summary>
    public void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            _logger.LogWarning("Attempted to cache a value with an empty key.");
            return;
        }

        try
        {
            var json = _serializer.Serialize(obj);
            var data = Encoding.UTF8.GetBytes(json);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            };

            _cache.Set(key, data, options);

            _logger.LogDebug("Cached key '{key}' with expirations: Abs={absoluteExpiration}, Sliding={slidingExpiration}",
                key, absoluteExpiration, slidingExpiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache key '{key}'.", key);
        }
    }

    /// <summary>
    /// Retrieves a cached value by key and deserializes it.
    /// </summary>
    public TOutput Get<TOutput>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        try
        {
            var json = _cache.GetString(key);

            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogDebug("Cache miss for key '{key}'.", key);
                return default;
            }

            var result = _serializer.Deserialize<TOutput>(json);

            _logger.LogDebug("Cache hit for key '{key}'.", key);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve cache for key '{key}'.", key);
            return default;
        }
    }

    /// <summary>
    /// Removes a cached value by key.
    /// </summary>
    public void RemoveCache(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        try
        {
            _cache.Remove(key);
            _logger.LogDebug("Cache entry removed for key '{key}'.", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove cache for key '{key}'.", key);
        }
    }
}
