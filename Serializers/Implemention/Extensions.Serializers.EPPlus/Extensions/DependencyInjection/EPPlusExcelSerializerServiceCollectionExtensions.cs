using Extensions.Serializers.Abstractions;
using Extensions.Serializers.EPPlus.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.EPPlus.Extensions.DependencyInjection;

public static class EPPlusExcelSerializerServiceCollectionExtensions
{
    public static IServiceCollection AddEPPlusExcelSerializer(this IServiceCollection services)
        => services.AddSingleton<IExcelSerializer, EPPlusExcelSerializer>();
}