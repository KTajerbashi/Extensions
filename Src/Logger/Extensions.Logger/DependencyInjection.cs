using Extensions.Logger.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Logger;

public static class DependencyInjection
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        return services;
    }
}
