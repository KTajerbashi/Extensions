using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApplicationAPI.DependencyInjections
{
    public static class DIContainer
    {
        public static IServiceCollection AddServiceWebApplication(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }
    }
}
