using Extensions.MessageBroker.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Extensions.MessageBroker.WebApi.Controllers;

public class UserMails
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}

[ApiController]
[Route("[controller]")]
public class MessageBrokerController : ControllerBase
{
    private readonly ILogger<MessageBrokerController> _logger;
    private readonly List<UserMails> _userMails;
    private readonly IMessageBroker _messageBroker;
    public MessageBrokerController(ILogger<MessageBrokerController> logger, IMessageBroker messageBroker)
    {
        _logger = logger;
        _userMails = new List<UserMails>()
        {
            new UserMails(){Id = 1,Name = "Tajerbahsi", Age = 12,Email ="Tajerbahsi@mail.com"},
            new UserMails(){Id = 2,Name = "Javad", Age = 16,Email ="Javad@mail.com"},
            new UserMails(){Id = 3,Name = "Jabbar", Age = 18,Email ="Jabbar@mail.com"},
            new UserMails(){Id = 4,Name = "Kazem", Age = 25,Email ="Kazem@mail.com"},
            new UserMails(){Id = 5,Name = "Reza", Age = 10,Email ="Reza@mail.com"},
            new UserMails(){Id = 6,Name = "Sharif", Age = 11,Email ="Sharif@mail.com"},
        };
        _messageBroker = messageBroker;
    }

    [HttpGet("Publish/{id}")]
    public async Task<IActionResult> Publish(int id)
    {
        await Task.CompletedTask;
        var entity = _userMails.Find(item => item.Id == id);
        await _messageBroker.PublishAsync(entity,"pub-sub", "pub-sub", "pub-sub");
        return Ok(new
        {
            Message = "Success",
            Model = entity
        });
    }

    [HttpGet("Subscribe")]
    public async Task<IActionResult> Subscribe()
    {
        await Task.CompletedTask;
        var result = await _messageBroker.SubscribeAsync<UserMails>("pub-sub","pub-sub","pub-sub",async (model) =>
        {
            await Task.CompletedTask;
            Console.WriteLine($"Controller => Id : {model.Id}");
            Console.WriteLine($"Controller => Name : {model.Name}");
            Console.WriteLine($"Controller => Age : {model.Age}");
            Console.WriteLine($"Controller => Email : {model.Email}");
            Console.WriteLine("=======================");
        });
        return Ok(new
        {
            Message = "Success",
            Model = "Default"
        });
    }

}
