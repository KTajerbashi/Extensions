using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.SharedKernel.Extensions;
using System.Text;

const string QueueKey = "HelloWorld"; // Must match sender's queue name

ApplicationType applicationType = ApplicationType.Receiver;
applicationType.Print($"[x] Receiver ~> [{DateTime.Now:G}] Start Application ...");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: QueueKey,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"~> {DateTime.Now:G} [x] Received: {message}");
    await Task.Yield(); // Optional: Simulate async work
};

// Use BasicConsume (not BasicConsumeAsync)
await channel.BasicConsumeAsync(queue: QueueKey, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

applicationType.Print($"[x] Receiver ~> [{DateTime.Now:G}] Finished Application ...");
