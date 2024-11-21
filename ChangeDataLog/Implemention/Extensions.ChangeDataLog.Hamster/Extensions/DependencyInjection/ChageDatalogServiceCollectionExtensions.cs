﻿using Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;
using Extensions.ChangeDataLog.Hamster.Options;

namespace Extensions.ChangeDataLog.Hamster.Extensions.DependencyInjection;

public static class ChageDatalogServiceCollectionExtensions
{
    public static IServiceCollection AddZaminHamsterChageDatalog(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ChangeDataLogHamsterOptions>(configuration);
        return services;
    }

    public static IServiceCollection AddZaminHamsterChageDatalog(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddZaminHamsterChageDatalog(configuration.GetSection(sectionName));
        return services;
    }

    public static IServiceCollection AddZaminHamsterChageDatalog(this IServiceCollection services, Action<ChangeDataLogHamsterOptions> setupAction)
    {
        services.Configure(setupAction);
        return services;
    }
}