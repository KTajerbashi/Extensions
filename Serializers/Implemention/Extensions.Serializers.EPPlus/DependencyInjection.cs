using Extensions.Serializers.Abstractions;
using Extensions.Serializers.EPPlus.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.Serializers.EPPlus;

public static class DependencyInjection
{
    public static IServiceCollection AddEPPlusExcelSerializer(this IServiceCollection services)
        => services.AddSingleton<IExcelSerializer, EPPlusExcelSerializer>();
}