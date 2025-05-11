
using Extensions.MessageBroker.Abstractions;
using Extensions.MessageBroker.WebApi.Controllers;

namespace Extensions.MessageBroker.WebApi;

public class MessageBrokerHost : BackgroundService
{
    private readonly IMessageBroker _messageBroker;
    public MessageBrokerHost(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var result = await _messageBroker.SubscribeAsync<UserMails>("pub-sub","pub-sub","pub-sub",async (model) =>
        {
            await Task.CompletedTask;
            Console.WriteLine($"Host => Id : {model.Id}");
            Console.WriteLine($"Host => Name : {model.Name}");
            Console.WriteLine($"Host => Age : {model.Age}");
            Console.WriteLine($"Host => Email : {model.Email}");
            Console.WriteLine("=======================");
        });
    }
}
