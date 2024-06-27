using Extensions.Caching.InMemory.Services;
using Microsoft.Extensions.DependencyInjection;
using Extensions.Caching.Abstractions;
using Extensions.Serializers.Abstractions;
using Extensions.Serializers.NewtonSoft.Services;

namespace Extensions.Caching.InMemory.Extensions.DependencyInjection;

/// <summary>
/// در ادامه مرحله چهارم
/// </summary>
public static class InMemoryCachingServiceCollectionExtensions
{

    public static IServiceCollection AddKernelInMemoryCaching(this IServiceCollection services)
        => services
        .AddMemoryCache()
        .AddTransient<ICacheAdapter, InMemoryCacheAdapter>()
        .AddSNewtonSoftSerializer();

    public static IServiceCollection AddSNewtonSoftSerializer(this IServiceCollection services)
        => services.AddSingleton<IJsonSerializer, NewtonSoftSerializer>();
}