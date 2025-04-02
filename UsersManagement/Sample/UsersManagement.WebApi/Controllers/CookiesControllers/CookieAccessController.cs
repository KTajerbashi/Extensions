using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

[Authorize]
public class CookieAccessController : BaseController
{
    public CookieAccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }

    [HttpGet("IsAdmin")]
    [Authorize(Roles ="Admin")]
    public IActionResult IsAdmin() => Ok("Access");

    [HttpGet("IsUser")]
    [Authorize(Roles ="User")]
    public IActionResult IsUser() => Ok("Access");
}


