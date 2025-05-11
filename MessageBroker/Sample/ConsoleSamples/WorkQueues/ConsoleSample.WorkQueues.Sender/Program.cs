using Framework.Basesource.Delegations;
using RabbitMQ.Client;
using System.Text;

await StartApplication.RunAsync("WorkQueues", Framework.Basesource.Extensions.ApplicationType.Sender, async () =>
{
    const string QueueName = "task_queue";
    const string RoutingKey = "task_queue";
    var factory = new ConnectionFactory { HostName = "localhost" };
    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false,
        autoDelete: false, arguments: null);

    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    var properties = new BasicProperties
    {
        Persistent = true
    };

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: RoutingKey, mandatory: true,
        basicProperties: properties, body: body);
    Console.WriteLine($" [x] Sent {message}");

    static string GetMessage(string[] args)
    {
        return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    }
});