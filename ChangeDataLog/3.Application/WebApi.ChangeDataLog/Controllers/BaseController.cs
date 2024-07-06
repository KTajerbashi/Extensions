using Microsoft.AspNetCore.Mvc;
using WebApi.ChangeDataLog.Models;

namespace WebApi.ChangeDataLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    protected readonly IHttpContextAccessor _contextAccessor;

    protected BaseController(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public virtual async Task<IActionResult> Ok<T>(Result<T> result)
    {
        if (result.StatusCode.Ok == StatusCodes.Status200OK)
        {
        }
        return View(result);
    }
}
