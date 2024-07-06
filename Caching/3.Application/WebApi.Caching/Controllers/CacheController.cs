using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Caching.Controllers;
public class InMemoryCacheController : BaseController
{
    public InMemoryCacheController(IHttpContextAccessor contextAccessor, ICacheAdapter cacheAdapter) : base(contextAccessor, cacheAdapter)
    {
    }

    [HttpPost]
    public IActionResult Add(string Key, string Value)
    {
        _cacheAdapter.Add(Key, Value, null, null);
        return Ok();
    }

    [HttpGet]
    public IActionResult Get(string Key)
    {
        return Ok(_cacheAdapter.Get<string>(Key));
    }

    [HttpDelete]
    public IActionResult Delete(string Key)
    {
        _cacheAdapter.RemoveCache(Key);
        return Ok();
    }
}