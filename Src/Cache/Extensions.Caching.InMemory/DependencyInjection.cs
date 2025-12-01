using Extensions.Caching.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Caching.InMemory;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryCacheAdapter(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheAdapter, InMemoryCacheAdapter>();
        return services;
    }
}



