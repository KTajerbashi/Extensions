using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.EPPlus;

public static class DependencyInjections
{
    public static IServiceCollection AddEPPlusSerializer(this IServiceCollection services)
    {
        services.AddSingleton<ISerializerExcel, SerializerEPPlusExcel>();
        return services;
    }
}
