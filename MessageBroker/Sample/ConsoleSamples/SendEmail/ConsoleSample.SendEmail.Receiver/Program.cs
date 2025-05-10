using Framework.Basesource.Apps.SendEmail.Data;
using Framework.Basesource.Delegations;
using Framework.Basesource.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
// Email Receiver Console Application
await StartApplication.RunAsync("Email System", ApplicationType.Receiver, async () =>
{
    var factory = new ConnectionFactory { HostName = "localhost" };
    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(
        queue: "email_queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    Console.WriteLine(" [*] Waiting for emails. Press Ctrl+C to exit.");

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += async (model, ea) =>
    {
        try
        {
            var body = ea.Body.ToArray();
            var emailJson = Encoding.UTF8.GetString(body);
            var email = JsonSerializer.Deserialize<EmailModel>(emailJson);

            if (email != null)
            {
                // Simulate email processing
                Console.WriteLine($" [x] Received email {email.Email}");
                Console.WriteLine($"     Subject: {email.Subject}");
                Console.WriteLine($"     Body: {email.ShortBody}");
                Console.WriteLine("     [Processing...]");
                await Task.Delay(1000); // Simulate work
                Console.WriteLine("     [Email processed successfully]");

                // Acknowledge the message
                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" [!] Error processing email: {ex.Message}");
        }
    };

    await channel.BasicConsumeAsync(
        queue: "email_queue",
        autoAck: false,
        consumer: consumer);

    // Keep receiver running
    await Task.Delay(Timeout.Infinite);
});