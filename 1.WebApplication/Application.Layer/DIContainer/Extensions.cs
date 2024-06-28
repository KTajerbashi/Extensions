using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Layer.DIContainer;

public static class Extensions
{

    #region Public
    
    public static IServiceCollection AddWithTransientLifetime(
        this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services
            .Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        return services;
    }
    #endregion


    
}
