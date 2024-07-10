using Microsoft.AspNetCore.Mvc;
using WebApi.ChangeDataLog.Models;

namespace WebApi.ChangeDataLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{

    protected BaseController()
    {
        
    }

    public async Task<IActionResult> ResultActionAsync<T>(Result<T> result)
    {
        if (result.StatusCode.Ok == StatusCodes.Status200OK)
        {
            return base.Ok(result);
        }
        return base.NotFound(result);
    }

    public Result<T> Result<T>(T result)
    {
        return new Result<T>
        {
            Data = result,
            Message = "",
            StatusCode = new StatusCode()
        };
    }
}
