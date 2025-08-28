using Microsoft.AspNetCore.Mvc;

namespace SampleSource.Common.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
}

