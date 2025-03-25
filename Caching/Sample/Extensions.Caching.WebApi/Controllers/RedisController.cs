using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Extensions.Caching.WebApi.Controllers;

public class RedisController : BaseController
{
    public RedisController(ICacheAdapter cacheAdapter) : base(cacheAdapter)
    {
    }
}
