using Application.Layer.DataAccess.ChangeDataLog;
using Extensions.Caching.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;

namespace WebApplicationAPI.Controllers.Caching
{
    public class SqlCachingController : BaseController
    {
        private readonly ICacheAdapter _cacheAdapter;
        public SqlCachingController(DatabaseContext context, ICacheAdapter cacheAdapter) : base(context)
        {
            _cacheAdapter = cacheAdapter;
        }

        [HttpPost]
        public IActionResult Add(string Key, string Value)
        {
            _cacheAdapter.Add(Key, Value, DateTime.Now.AddDays(1), null);
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
