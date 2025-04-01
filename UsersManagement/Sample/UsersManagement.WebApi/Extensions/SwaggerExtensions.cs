namespace UsersManagement.WebApi.Extensions;

public class SwaggerOption
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
}
public static class SwaggerExtensions
{
    public static IServiceCollection Add(this IServiceCollection services)
    {
        services.AddSwagger(option =>
        {
            option.Title = "";
            option.Name = "";
            option.Description = "";
            option.Version = "";
        });
        return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services, Action<SwaggerOption> action)
    {
        services.Configure(action);

        return services;
    }
}
