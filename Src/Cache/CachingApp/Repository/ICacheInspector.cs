using CachingApp.Models;

namespace CachingApp.Repository;

public interface ICacheInspector
{
    List<CacheRecord> GetAll();
}
