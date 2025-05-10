using Framework.Basesource.Delegations;
using Framework.Basesource.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Framework.Basesource.RabbitMQServices;

public class MessageSystem
{
    private string RoutingKey = string.Empty;
    private string QueueName = string.Empty;
    private readonly ApplicationType _applicationType;

    public MessageSystem(
        ApplicationType applicationType,
        string routingKey,
        string queueName
        )
    {
        _applicationType = applicationType;
        RoutingKey = routingKey;
        QueueName = queueName;
    }

    public async Task RunAsync()
    {
        await StartApplication.RunAsync("MessageSystem", _applicationType, async () =>
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync(); // Changed to CreateChannel

            await InitializeQueueAsync(channel);

            if (_applicationType == ApplicationType.Sender)
            {
                await RunSenderAsync(channel);
            }
            else
            {
                await RunReceiverAsync(channel);
            }
        });
    }

    private async Task InitializeQueueAsync(IChannel channel)
    {
        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    private async Task RunSenderAsync(IChannel channel)
    {
        Console.WriteLine("Message Sender started. Type 'exit' to quit.");

        while (true)
        {
            Console.Write("Enter Message: ");
            var userMessage = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userMessage))
                continue;

            if (userMessage.ToLower() == "exit")
                break;

            try
            {
                await SendMessageAsync(channel, userMessage);
                Console.WriteLine($"~> {DateTime.Now:G} [x] Sent {userMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }

    private async Task SendMessageAsync(IChannel channel, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: RoutingKey,
            body: body);
    }

    private async Task RunReceiverAsync(IChannel channel)
    {
        Console.WriteLine("Message Receiver started. Press Ctrl+C to exit.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"~> {DateTime.Now:G} [x] Received {message}");
            await Task.Yield();
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: true,
            consumer: consumer);

        // Keep the receiver running
        await Task.Delay(Timeout.Infinite);
    }
}