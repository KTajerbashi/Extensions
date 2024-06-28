using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.Base.Events;
using Application.Layer.Model.Base.Queries;
using Extensions.DependencyInjection.Abstractions;
using Extensions.Serializers.NewtonSoft.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using Extensions.Serializers.NewtonSoft.Extensions.DependencyInjection;

namespace Application.Layer.DIContainer;

public static class DIContainer
{
    public static IServiceCollection AddServiceWebApplication(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddNewtonSoftSerializer();
        services.AddDependencies("WebApplicationAPI", "Application");
        return services;
    }
    public static ICommandDispatcher CommandDispatcher(this HttpContext httpContext) =>
    (ICommandDispatcher)httpContext.RequestServices.GetService(typeof(ICommandDispatcher));

    public static IQueryDispatcher QueryDispatcher(this HttpContext httpContext) =>
        (IQueryDispatcher)httpContext.RequestServices.GetService(typeof(IQueryDispatcher));
    public static IEventDispatcher EventDispatcher(this HttpContext httpContext) =>
        (IEventDispatcher)httpContext.RequestServices.GetService(typeof(IEventDispatcher));
    //public static UtilitiesServices ApplicationContext(this HttpContext httpContext) =>
    //    (UtilitiesServices)httpContext.RequestServices.GetService(typeof(UtilitiesServices));

    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        params string[] assemblyNamesForSearch)
    {

        var assemblies = GetAssemblies(assemblyNamesForSearch);
        services
            .AddApplicationServices(assemblies)
            .AddCustomDependencies(assemblies);
        return services;
    }
    private static List<Assembly> GetAssemblies(string[] assemblyName)
    {
        var assemblies = new List<Assembly>();
        var dependencies = DependencyContext.Default.RuntimeLibraries;
        foreach (var library in dependencies)
        {
            if (IsCandidateCompilationLibrary(library, assemblyName))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
        }
        return assemblies;
    }
    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assemblyName)
    {
        return assemblyName.Any(d => compilationLibrary.Name.Contains(d))
            || compilationLibrary.Dependencies.Any(d => assemblyName.Any(c => d.Name.Contains(c)));
    }
    public static IServiceCollection AddCustomDependencies(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        return services
                        .AddWithTransientLifetime(assemblies, typeof(ITransientLifetime))
                        .AddWithScopedLifetime(assemblies, typeof(IScopeLifetime))
                        .AddWithSingletonLifetime(assemblies, typeof(ISingletoneLifetime));
    }
    public static IServiceCollection AddWithScopedLifetime(
        this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        return services;
    }
    public static IServiceCollection AddWithSingletonLifetime(
        this IServiceCollection services,
        IEnumerable<Assembly> assembliesForSearch,
        params Type[] assignableTo)
    {
        services.Scan(s => s.FromAssemblies(assembliesForSearch)
            .AddClasses(c => c.AssignableToAny(assignableTo))
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
        return services;
    }

}


