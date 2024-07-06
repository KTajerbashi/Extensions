using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Caching.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    protected readonly IHttpContextAccessor _contextAccessor;
    protected readonly ICacheAdapter _cacheAdapter;

    protected BaseController(IHttpContextAccessor contextAccessor, ICacheAdapter cacheAdapter)
    {
        _contextAccessor = contextAccessor;
        _cacheAdapter = cacheAdapter;
    }
}
