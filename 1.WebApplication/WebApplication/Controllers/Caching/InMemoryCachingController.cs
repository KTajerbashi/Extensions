using Application.Layer.DataAccess.ChangeDataLog;
using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;

namespace WebApplicationAPI.Controllers.Caching
{
    public class InMemoryCachingController : BaseController
    {
        private readonly ICacheAdapter _cacheAdapter;
        public InMemoryCachingController(DatabaseContext context, ICacheAdapter cacheAdapter) : base(context)
        {
            _cacheAdapter = cacheAdapter;
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
}
