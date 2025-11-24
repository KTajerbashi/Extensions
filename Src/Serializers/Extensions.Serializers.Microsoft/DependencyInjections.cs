using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.Microsoft;

public static class DependencyInjections
{
    public static IServiceCollection AddMicrosoftSerializers(this IServiceCollection services)
    {
        services.AddSingleton<ISerializerJson, SerializerMicrosoft>();
        return services;
    }
}
