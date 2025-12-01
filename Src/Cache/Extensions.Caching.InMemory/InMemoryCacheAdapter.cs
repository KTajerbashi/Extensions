using Extensions.Caching.Abstractions;
using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Extensions.Caching.InMemory;

public class InMemoryCacheAdapter : ICacheAdapter
{
    private readonly IMemoryCache _memoryCache;
    private readonly ISerializerJson _serializer;
    private readonly ILogger<InMemoryCacheAdapter> _logger;

    public InMemoryCacheAdapter(
        IMemoryCache memoryCache,
        ISerializerJson serializer,
        ILogger<InMemoryCacheAdapter> logger)
    {
        _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _logger.LogInformation("InMemoryCacheAdapter initialized.");
    }

    public void Add<TInput>(string key, TInput value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));

        var options = BuildCacheOptions(absoluteExpiration, slidingExpiration);

        SafeLogCacheOperation("Add", key, value);

        _memoryCache.Set(key, value, options);
    }

    public TOutput Get<TOutput>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return default;

        _logger.LogTrace("Attempting to retrieve cache entry with key: {Key}", key);

        if (_memoryCache.TryGetValue(key, out TOutput value))
        {
            SafeLogCacheOperation("Hit", key, value);
            return value;
        }

        _logger.LogTrace("Cache miss for key: {Key}", key);
        return default;
    }

    public void RemoveCache(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        _logger.LogTrace("Removing cache entry with key: {Key}", key);
        _memoryCache.Remove(key);
    }

    // -------------------------------------------------
    // Helper methods
    // -------------------------------------------------
    private MemoryCacheEntryOptions BuildCacheOptions(DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        var options = new MemoryCacheEntryOptions();

        if (absoluteExpiration.HasValue)
            options.AbsoluteExpiration = absoluteExpiration;

        if (slidingExpiration.HasValue)
            options.SlidingExpiration = slidingExpiration;

        return options;
    }

    private void SafeLogCacheOperation<T>(string operation, string key, T value)
    {
        try
        {
            var serialized = _serializer.Serialize(value);
            _logger.LogTrace("Cache {Operation} - Key: {Key}, Data: {Data}", operation, key, serialized);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Serialization failed while logging cache {Operation} for key: {Key}.", operation, key);
        }
    }
}



