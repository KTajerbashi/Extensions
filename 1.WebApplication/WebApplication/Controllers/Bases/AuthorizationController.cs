using Application.Layer.DataAccess.ChangeDataLog;
using Microsoft.AspNetCore.Authorization;

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
