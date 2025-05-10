using Framework.Basesource.Apps.SendEmail.Data;
using Framework.Basesource.Delegations;
using Framework.Basesource.Extensions;
using System.Text;
using System.Text.Json;

await StartApplication.RunAsync("Email System", ApplicationType.Sender, async () =>
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

    Console.WriteLine("User Registration System (type 'exit' to finish)");
    Console.WriteLine("-----------------------------------------------");

    while (true)
    {
        Console.Write("Enter user details or 'exit' to finish:\n");

        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        if (ShouldExit(firstName)) break;

        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        if (ShouldExit(lastName)) break;

        Console.Write("Email: ");
        var email = Console.ReadLine();
        if (ShouldExit(email)) break;

        // Validate email format
        if (!IsValidEmail(email))
        {
            Console.WriteLine("Invalid email format. Please try again.");
            continue;
        }

        // Add user and create welcome email
        var user = DatabaseContext.Instance.AddUser(new UserModel(firstName, lastName, email));
        var welcomeEmail = new EmailModel(
            user.Email,
            $"Welcome {user.Name}!",
            $"Dear {user.Name} {user.Family},\n\nThank you for registering with us!");

        DatabaseContext.Instance.AddEmail(welcomeEmail);

        Console.WriteLine($"User '{user.Name} {user.Family}' added successfully!");
        Console.WriteLine($"Welcome email queued for {user.Email}\n");

        Console.WriteLine("\nSending all queued emails...");

        // Process and send all pending emails
        while (DatabaseContext.Instance.HasPendingEmails)
        {
            var emailentity = DatabaseContext.Instance.DequeueEmail();
            if (emailentity == null) continue;

            try
            {
                var emailJson = JsonSerializer.Serialize(emailentity);
                var body = Encoding.UTF8.GetBytes(emailJson);

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "email_queue",
                    body: body);

                emailentity.Status = EmailStatus.Sent;
                DatabaseContext.Instance.UpdateEmail(emailentity);

                Console.WriteLine($" [✓] Sent email to {emailentity.Email}");
            }
            catch (Exception ex)
            {
                emailentity.Status = EmailStatus.Failed;
                DatabaseContext.Instance.UpdateEmail(emailentity);
                Console.WriteLine($" [✗] Failed to send email to {emailentity.Email}: {ex.Message}");
            }
        }
    }




    Console.WriteLine("\nAll emails processed. Press any key to exit...");
    Console.ReadKey();
});

bool ShouldExit(string? input) => string.IsNullOrWhiteSpace(input) ||
                               input.Equals("exit", StringComparison.OrdinalIgnoreCase);

bool IsValidEmail(string email)
{
    try
    {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
    }
    catch
    {
        return false;
    }
}