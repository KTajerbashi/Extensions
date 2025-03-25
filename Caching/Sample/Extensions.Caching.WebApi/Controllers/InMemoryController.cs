using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Extensions.Caching.WebApi.Controllers;

public class InMemoryController : BaseController
{
    public InMemoryController(ICacheAdapter cacheAdapter) : base(cacheAdapter)
    {
    }
}
