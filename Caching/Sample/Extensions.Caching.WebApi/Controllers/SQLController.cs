using Extensions.Caching.Abstractions;

namespace Extensions.Caching.WebApi.Controllers;
public class SQLController : BaseController
{
    public SQLController(ICacheAdapter cacheAdapter) : base(cacheAdapter)
    {
    }
}
