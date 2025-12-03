using Extensions.Caching.Abstractions;
using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Extensions.Caching.InMemory;
/// <summary>
///     In-memory cache adapter implementation using <see cref="IMemoryCache"/>.
///     This class provides a clean abstraction for caching operations and is suitable
///     for Clean Architecture applications.
/// </summary>
public class InMemoryCacheAdapter : ICacheAdapter
{
    private readonly IMemoryCache _memoryCache;
    private readonly ISerializerJson _serializer;
    private readonly ILogger<InMemoryCacheAdapter> _logger;

    /// <summary>
    ///     Initializes a new instance of <see cref="InMemoryCacheAdapter"/>.
    /// </summary>
    /// <param name="memoryCache">The in-memory cache provider.</param>
    /// <param name="serializer">Serializer used for logging cached objects.</param>
    /// <param name="logger">Logger instance for diagnostics.</param>
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

    // -------------------------------------------------------------------
    // ADD
    // -------------------------------------------------------------------

    /// <summary>
    ///     Adds a value to the cache with optional expiration settings.
    /// </summary>
    /// <typeparam name="TInput">The type of the object being cached.</typeparam>
    /// <param name="key">The unique cache key.</param>
    /// <param name="value">The value to be cached.</param>
    /// <param name="absoluteExpiration">Optional absolute expiration timestamp.</param>
    /// <param name="slidingExpiration">Optional sliding expiration duration.</param>
    /// <exception cref="ArgumentException">Thrown when key is null or empty.</exception>
    /// <remarks>
    ///     Use this method when you want to add or update a cached entry manually.
    ///     If both expiration values are null, the item will stay in cache
    ///     until evicted by the system or memory pressure rules.
    ///
    ///     <example>
    ///     cache.Add("users_all", usersList, DateTime.UtcNow.AddMinutes(10), null);
    ///     </example>
    /// </remarks>
    public void Add<TInput>(string key, TInput value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));

        var options = BuildCacheOptions(absoluteExpiration, slidingExpiration);

        SafeLogCacheOperation("Add", key, value);

        _memoryCache.Set(key, value, options);
    }

    // -------------------------------------------------------------------
    // GET
    // -------------------------------------------------------------------

    /// <summary>
    ///     Retrieves a value from the cache if available; otherwise returns default.
    /// </summary>
    /// <typeparam name="TOutput">Expected type of the cached value.</typeparam>
    /// <param name="key">The unique cache key.</param>
    /// <returns>
    ///     Cached object if found; otherwise default(TOutput).
    /// </returns>
    /// <remarks>
    ///     Returns a cache hit or miss without throwing exceptions.
    ///
    ///     <example>
    ///     var users = cache.Get&lt;List&lt;User&gt;&gt;("users_all");
    ///     </example>
    /// </remarks>
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

    // -------------------------------------------------------------------
    // REMOVE
    // -------------------------------------------------------------------

    /// <summary>
    ///     Removes a cached entry using the provided key.
    /// </summary>
    /// <param name="key">The unique cache key.</param>
    /// <remarks>
    ///     Safe to call even if the key does not exist.
    ///
    ///     <example>
    ///     cache.RemoveCache("user_profile_101");
    ///     </example>
    /// </remarks>
    public void RemoveCache(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        _logger.LogTrace("Removing cache entry with key: {Key}", key);
        _memoryCache.Remove(key);
    }

    // -------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------

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
