using Microsoft.AspNetCore.Mvc;
using System.Formats.Tar;
using WebApplicationAPI.DataAccess.ChangeDataLog;

namespace WebApplicationAPI.Controllers.Bases
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        protected readonly DatabaseContext context;
        protected BaseController(DatabaseContext context)
        {
            this.context = context;
        }
    }
}
