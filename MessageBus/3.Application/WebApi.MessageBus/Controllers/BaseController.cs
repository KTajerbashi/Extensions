using Extensions.MessageBus.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.MessageBus.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly ISendMessageBus _sendMessageBus;
    protected BaseController(ISendMessageBus sendMessageBus)
    {
        _sendMessageBus = sendMessageBus;
    }
}
