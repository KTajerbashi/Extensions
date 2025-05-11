using Framework.Basesource.Delegations;
using RabbitMQ.Client;
using System.Text;

await StartApplication.RunAsync(
    "PublishSubscribe",
    Framework.Basesource.Extensions.ApplicationType.Sender,
    async () => {

        const string ExchangeName = "logs";

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: ExchangeName, 
            type: ExchangeType.Fanout);

        var message = GetMessage(args);
        var body = Encoding.UTF8.GetBytes(message);
        await Task.Delay(5000);
        await channel.BasicPublishAsync(
            exchange: ExchangeName, 
            routingKey: string.Empty, 
            body: body);
        Console.WriteLine($" [x] Sent {message}");

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();

        static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }

    });