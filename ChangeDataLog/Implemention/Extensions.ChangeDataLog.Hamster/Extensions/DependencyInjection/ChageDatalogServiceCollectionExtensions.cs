using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Hamster.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;

public static class ChageDatalogServiceCollectionExtensions
{
    public static IServiceCollection AddHamsterChageDatalog(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ChangeDataLogHamsterOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddHamsterChageDatalog(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddHamsterChageDatalog(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddHamsterChageDatalog(this IServiceCollection services, Action<ChangeDataLogHamsterOptions> setupAction)
    {
        services.Configure(setupAction);
        return services;
    }
}