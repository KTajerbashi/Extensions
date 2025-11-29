using Extensions.Logger.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Logger;

public static class DependencyInjection
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        services.AddTransient<IAppLogger, AppLoggerAdapter>();
        services.AddTransient(typeof(IAppLogger<>), typeof(AppLoggerAdapter<>));
        return services;
    }
}
