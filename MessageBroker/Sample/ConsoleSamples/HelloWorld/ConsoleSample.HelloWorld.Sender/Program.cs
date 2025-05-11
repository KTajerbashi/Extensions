using ConsoleSample.HelloWorld.Sender;
using Framework.Basesource.Extensions;
using RabbitMQ.Client;
using System.Text;

const string RoutingKey = "HelloWorld";
const string QueueName = "HelloWorld";

ApplicationType applicationType = ApplicationType.Sender;
applicationType.Print($"[x] Sender ~> [{DateTime.Now:G}] Start Application ...");

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: QueueName, durable: false, exclusive: false, autoDelete: false,
    arguments: null);

bool retry;
do
{
    var messages = MessageModel.GetMessages();
    foreach (var item in messages)
    {
        var body = Encoding.UTF8.GetBytes($"{item.Message} FOR {item.Name}");
        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: RoutingKey, body: body);
        Console.WriteLine($"~> {DateTime.Now:G} [x] Sent {item.Message} For {item.Name}");
        await Task.Delay(3000);
    }

    Console.WriteLine("Do you want to resend the messages? (Y/N):");
    var input = Console.ReadLine();
    retry = input?.Trim().Equals("Y", StringComparison.OrdinalIgnoreCase) == true;

} while (retry);

applicationType.Print($"[x] Sender ~> [{DateTime.Now:G}] Finished Application ...");
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
