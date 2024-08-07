﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace MediateR.Extensions.Configurations.DependencyInjection;

public static class MediateRInjection
{
    public static IServiceCollection AddDependenciesMediateR(this IServiceCollection services, string assemblyNames)
        => services.AddMediateRServices(assemblyNames);


    private static IServiceCollection AddMediateRServices(this IServiceCollection services, string assemblyNames)
    {
        services.AddMediatR(options =>
        {
            foreach (var assemblyName in GetAssemblies(assemblyNames))
            {
                options.RegisterServicesFromAssemblies(assemblyName);
            }
        });
        return services;
    }

    private static List<Assembly> GetAssemblies(string assemblyNames)
    {
        var assemblies = new List<Assembly>();
        var dependencies = DependencyContext.Default.RuntimeLibraries;

        foreach (var library in dependencies)
        {
            if (IsCandidateCompilationLibrary(library, assemblyNames.Split(',')))
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                assemblies.Add(assembly);
            }
        }

        return assemblies;
    }

    private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary, string[] assemblyName)
    => assemblyName.Any(d => compilationLibrary.Name.Contains(d)) || compilationLibrary.Dependencies.Any(d => assemblyName.Any(c => d.Name.Contains(c)));

}
