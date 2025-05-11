using Framework.Basesource.Delegations;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

await StartApplication.RunAsync(
    "PublishSubscribe",
    Framework.Basesource.Extensions.ApplicationType.Receiver,
    async () => {

        const string ExchangeName = "logs";
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: ExchangeName,
            type: ExchangeType.Fanout);

        // declare a server-named queue
        QueueDeclareOk queueDeclareResult = await channel.QueueDeclareAsync();
        string queueName = queueDeclareResult.QueueName;
        await channel.QueueBindAsync(
            queue: queueName, 
            exchange: ExchangeName, 
            routingKey: string.Empty);

        Console.WriteLine(" [*] Waiting for logs.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] {message}");
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();

    });