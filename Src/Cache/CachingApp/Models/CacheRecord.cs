namespace CachingApp.Models;

public class CacheRecord
{
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTimeOffset ExpiresAtTime { get; set; }
    public long? SlidingExpirationInSeconds { get; set; }
    public DateTimeOffset? AbsoluteExpiration { get; set; }

    public bool IsActive => ExpiresAtTime > DateTimeOffset.UtcNow;
}
