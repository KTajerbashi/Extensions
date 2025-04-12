using RabbitMQ.Client;
using System.Text;

namespace HelloWorld.Sender;


public class Producer
{
    private const string QueueName = "hello";
    private const string HostName = "localhost";

    public static async Task StartAsync()
    {
        string message = "";
        while (message != "exit")
        {
            Console.Write("Enter Message : ");
            message = Console.ReadLine();
            var factory = new ConnectionFactory { HostName = HostName };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync("", QueueName, body, CancellationToken.None);

            Console.WriteLine($" [x] Sent: {message}");
        }
    }
}




