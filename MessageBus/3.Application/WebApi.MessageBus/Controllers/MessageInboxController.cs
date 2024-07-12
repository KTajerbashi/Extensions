using Extensions.MessageBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebApi.MessageBus.Models;

namespace WebApi.MessageBus.Controllers;

public class MessageInboxController : BaseController
{
    public MessageInboxController(ISendMessageBus sendMessageBus) : base(sendMessageBus)
    {
    }

    [HttpPost("SendEvent")]
    public IActionResult SendEvent([FromBody] PersonEvent personEvent)
    {
        _sendMessageBus.Publish(personEvent);
        return Ok();
    }

    [HttpPost("SendCommand")]
    public IActionResult SendCommand([FromBody] PersonCommand PersonCommand)
    {
        _sendMessageBus.SendCommandTo("MessageBus", "PersonCommand", PersonCommand);
        return Ok();
    }
}
