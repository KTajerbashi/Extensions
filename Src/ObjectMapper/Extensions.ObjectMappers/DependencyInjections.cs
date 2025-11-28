using AutoMapper;
using Extensions.ObjectMappers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace Extensions.ObjectMappers;

/// <summary>
/// Provides extension methods for registering AutoMapper and custom mapping adapters
/// into the application's dependency injection container.
/// </summary>
public static class DependencyInjections
{
    /// <summary>
    /// Adds AutoMapper configuration by scanning the provided assemblies for mapping profiles,
    /// and registers the application's mapping adapter (<see cref="IMapperAdapter"/>).
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="assemblies">Assemblies containing AutoMapper <see cref="Profile"/> classes.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAutoMapperProfiles(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        return services
            .AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assemblies);
            })
            .AddSingleton<IMapperAdapter, ObjectMapperAdapter>();
    }

    public static IServiceCollection AddAutoMapperProfiles(
     this IServiceCollection services,
     params string[] projectNamespace)
    {
        List<Assembly> assemblies = new();
        foreach (var name in projectNamespace)
        {
            assemblies.AddRange(name.GetAssemblies());
        }
        return services
            .AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assemblies);
            })
            .AddSingleton<IMapperAdapter, ObjectMapperAdapter>();
    }

    /// <summary>
    /// Adds AutoMapper by loading mapping profiles from a given type's assembly.
    /// Useful when you want to load all profiles from a feature/module assembly.
    /// </summary>
    /// <typeparam name="T">Any type inside the target assembly.</typeparam>
    /// <param name="services">The DI service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAutoMapperFromAssemblyOf<T>(
        this IServiceCollection services)
    {
        return services
            .AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(T).Assembly);
            })
            .AddSingleton<IMapperAdapter, ObjectMapperAdapter>();
    }

    /// <summary>
    /// Adds AutoMapper and allows defining inline mapping profiles directly in code.
    /// Useful for small apps or API-only projects.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="configure">A configuration action for AutoMapper.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAutoMapperWithCustomConfig(
        this IServiceCollection services,
        Action<IMapperConfigurationExpression> configure)
    {
        return services
            .AddAutoMapper(cfg =>
            {
                configure(cfg);
            })
            .AddSingleton<IMapperAdapter, ObjectMapperAdapter>();
    }

    /// <summary>
    /// Adds AutoMapper with advanced configuration options, including validation,
    /// naming conventions, constructor mapping, and more.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="assemblies">Assemblies to scan for Profiles.</param>
    /// <param name="advancedConfig">Extra AutoMapper configuration options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAutoMapperAdvanced(
        this IServiceCollection services,
        Assembly[] assemblies,
        Action<IMapperConfigurationExpression> advancedConfig)
    {
        return services
            .AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assemblies);
                advancedConfig(cfg);
            })
            .AddSingleton<IMapperAdapter, ObjectMapperAdapter>();
    }

    public static List<Assembly> GetAssemblies(this string namespaceProject)
    {
        var assemblies = new List<Assembly>();
        var dependencies = DependencyContext.Default.RuntimeLibraries;

        foreach (var library in dependencies)
        {
            if (IsCandidateCompilationLibrary(library, namespaceProject.Split(',')))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
        }

        return assemblies;
    }
    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assmblyName)
       => assmblyName.Any(d => compilationLibrary.Name.Contains(d))
          || compilationLibrary.Dependencies.Any(d => assmblyName.Any(c => d.Name.Contains(c)));

}
