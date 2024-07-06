using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Caching.Controllers;

public class RedisController : BaseController
{
    public RedisController(IHttpContextAccessor contextAccessor, ICacheAdapter cacheAdapter) : base(contextAccessor, cacheAdapter)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Add()
    {
        if (!ModelState.IsValid)
        {
            await Task.Delay(1000);
        }
        return Ok("Done");
    }

    [HttpPut]
    public async Task<IActionResult> Update()
    {
        if (!ModelState.IsValid)
        {
            await Task.Delay(1000);
        }
        return Ok("Done");
    }

    [HttpDelete]
    public async Task<IActionResult> Remove()
    {
        if (!ModelState.IsValid)
        {
            await Task.Delay(1000);
        }
        return Ok("Done");
    }

}
