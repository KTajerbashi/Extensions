using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.NewtonSoft;

public static class DependencyInjections
{
    public static IServiceCollection AddNewtonSoftSerializer(this IServiceCollection services)
    {
        services.AddSingleton<ISerializerJson, SerializerNewtonSoft>();
        return services;
    }
}
