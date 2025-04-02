using Microsoft.AspNetCore.Authorization;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

[Authorize]
public class AccessController : BaseController
{
    public AccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }
}


