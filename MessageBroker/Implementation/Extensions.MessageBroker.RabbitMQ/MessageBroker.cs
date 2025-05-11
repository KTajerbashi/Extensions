using Extensions.MessageBroker.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Extensions.MessageBroker.RabbitMQ;

public class MessageBroker : IMessageBroker
{
    private readonly string _hostName = "localhost";

    public async Task PublishAsync<T>(T data, string queueName, string exchangeName, string routingKey)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName, 
            type: ExchangeType.Fanout);

        var message = JsonSerializer.Serialize(data);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: string.Empty, // Fanout ignores routing key
            body: body);
    }

    public async Task<T> SubscribeAsync<T>(string queueName, string exchangeName, string routingKey, Func<T, Task> onMessage)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);

        var queueDeclareOk = await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj = JsonSerializer.Deserialize<T>(message);
            if (obj is not null)
                await onMessage(obj);
        };

         await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

        return default;
    }


}