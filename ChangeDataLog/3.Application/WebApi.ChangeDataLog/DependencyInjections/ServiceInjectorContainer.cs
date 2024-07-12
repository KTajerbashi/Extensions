using Extensions.ChangeDataLog.Hamster.Interceptors;
using Microsoft.EntityFrameworkCore;
using WebApi.ChangeDataLog.Database;
using WebApi.ChangeDataLog.Repositories.Users;
using WebApi.ChangeDataLog.Services.Users;

namespace WebApi.ChangeDataLog.DependencyInjections;

public static class ServiceInjectorContainer
{
    public static IServiceCollection AddDependencies(this IServiceCollection services,IConfiguration configuration)
    {
        return services.AddRepositories().AddDatabase(configuration);
    }
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserService>();
        return services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .AddInterceptors(new AddChangeDataLogInterceptor());
        });
        return services;
    }
}
