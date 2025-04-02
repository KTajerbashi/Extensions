using Extensions.UsersManagement.Abstractions;
using Extensions.UsersManagement.Options;
using Extensions.UsersManagement.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.UsersManagement;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersManagement(this IServiceCollection services, IConfiguration configuration, bool useFake = false)
    {
        if (useFake)
        {
            services.AddSingleton(typeof(IUserManager<>), typeof(UserManagerFake));
        }
        else
        {
            services.Configure<UserManagementOptions>(configuration);
            services.AddSingleton(typeof(IUserManager<>), typeof(UserManager));

        }
        return services;
    }


    public static IServiceCollection AddUsersManagement(this IServiceCollection services, IConfiguration configuration, string sectionName, bool useFake = false)
    {
        services.AddUsersManagement(configuration.GetSection(sectionName), useFake);
        return services;
    }

    public static IServiceCollection AddUsersManagement(this IServiceCollection services, Action<UserManagementOptions> setupAction, bool useFake = false)
    {
        if (useFake)
            services.AddSingleton(typeof(IUserManager<>), typeof(UserManagerFake));
        else
        {
            services.Configure(setupAction);
            services.AddSingleton(typeof(IUserManager<>), typeof(UserManager));
        }
        return services;
    }



}
