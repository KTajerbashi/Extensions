using Extensions.MessageBroker.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Extensions.MessageBroker.RabbitMQ;

public static class DependencyInjections
{
    public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, MessageBroker>();
        return services;
    }
}