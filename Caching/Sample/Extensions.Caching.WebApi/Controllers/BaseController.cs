using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Extensions.Caching.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : Controller
{
    private readonly ICacheAdapter _cacheAdapter;
    protected BaseController(ICacheAdapter cacheAdapter)
    {
        _cacheAdapter = cacheAdapter;
    }
    [HttpGet("Add")]
    public IActionResult Add(string key, string value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
    {
        _cacheAdapter.Add(key, value, absoluteExpiration, slidingExpiration);
        return Ok("Added");
    }

    [HttpGet("Get")]
    public IActionResult Get(string key)
    {
        return Ok(_cacheAdapter.Get<string>(key));
    }
    [HttpGet("Remove")]
    public IActionResult Remove(string key)
    {
        _cacheAdapter.RemoveCache(key);
        return Ok("Removed");
    }
}
