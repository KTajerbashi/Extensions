using FluentEmail.Core;
using FluentEmail.Smtp;
using SampleSource.Providers.AutoFacDI;
using System.Net;
using System.Net.Mail;

namespace SampleSource.Providers.FluentEmails;


public static class DependencyInjections
{
    public static WebApplicationBuilder AddFluenEmails(this WebApplicationBuilder builder)
    {
        // Get settings from configuration - HIGHLY RECOMMENDED
        var smtpSettings = builder.Configuration.GetSection("SmtpSettings");
        string fromAddress = smtpSettings["FromAddress"] ?? "tajersystem@gmail.com";
        string server = smtpSettings["Server"] ?? "smtp.gmail.com";
        int port = int.Parse(smtpSettings["Port"] ?? "587");
        string username = smtpSettings["Username"] ?? "kamrantajerbashi@gmail.com";
        string password = smtpSettings["Password"] ?? "ifeo zyfi njxl tjxw";

        builder.Services.AddFluentEmail(fromAddress)
               .AddRazorRenderer()
               .AddSmtpSender(new SmtpClient(server)
               {
                   Port = port,
                   Credentials = new NetworkCredential(username, password),
                   EnableSsl = true, // Crucial for Gmail
                   UseDefaultCredentials = false // Ensure custom credentials are used
               });

        return builder;
    }
}


public class WelcomeEmailModel
{
    public string Name { get; set; }
    public DateTime SignupDate { get; set; }
}
public class EmailService : IAutofacScopedLifetime
{
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task SendWelcomeEmail(string recipientName, string recipientAddress)
    {
        await _fluentEmail
            .To(recipientAddress)
            .Subject($"Welcome {recipientName}!")
            .Body($"Hi {recipientName}, <br> We are thrilled to have you.", isHtml: true)
            .SendAsync();
    }

    public async Task RunAsync()
    {
        var model = new WelcomeEmailModel { Name = "John Doe", SignupDate = DateTime.UtcNow };

        var email = _fluentEmail
            .To("TajerSystem@gmail.com")
            .Subject("Welcome!")
            .UsingTemplate(@"
        <div>
        <h1>Welcome</h1>
        <p>This an Email From Sample Solution ...</p>
        </div>
",model,true)
            //.UsingTemplateFromEmbedded("MyApp.Templates.WelcomeEmail.cshtml", model, GetType().Assembly)
            ;

        await email.SendAsync();
    }
}