using Extensions.Serializers.Abstractions;
using Extensions.Serializers.NewtonSoft.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.NewtonSoft.Extensions.DependencyInjection;
public static class NewtonSoftSerializerServiceCollectionExtensions
{
    public static IServiceCollection AddNewtonSoftSerializer(this IServiceCollection services)
        => services.AddSingleton<IJsonSerializer, NewtonSoftSerializer>();
}