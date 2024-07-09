using Microsoft.AspNetCore.Mvc;

namespace WebApi.MessageBus.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{

}


public class MessageInboxController : BaseController
{

}
