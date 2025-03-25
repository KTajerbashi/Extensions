using Extensions.Caching.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Caching.InMemory;
public static class DependencyInjections
{
    public static IServiceCollection AddInMemoryCaching(this IServiceCollection services)
        => services.AddMemoryCache().AddTransient<ICacheAdapter, InMemoryCacheAdapter>();

}

