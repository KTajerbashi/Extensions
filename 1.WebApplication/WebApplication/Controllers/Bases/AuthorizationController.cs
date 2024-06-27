using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.DataAccess.ChangeDataLog;

namespace WebApplicationAPI.Controllers.Bases
{
    [Authorize]
    public abstract class AuthorizationController : BaseController
    {
        protected AuthorizationController(DatabaseContext context) : base(context)
        {
        }
    }
}
