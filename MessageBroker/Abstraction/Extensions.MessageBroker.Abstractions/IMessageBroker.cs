namespace Extensions.MessageBroker.Abstractions;

public interface IMessageBroker
{
    Task PublishAsync<T>(T data, string queueName, string exchangeName, string routingKey);

    Task<T> SubscribeAsync<T>(string queueName, string exchangeName, string routingKey, Func<T, Task> onMessage);

}