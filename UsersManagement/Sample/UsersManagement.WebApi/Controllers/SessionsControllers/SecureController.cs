using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.SessionsControllers;

[Authorize] // This applies to all actions in the controller
public class SessionSecureController : BaseController
{
    public SessionSecureController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("This is a secure endpoint");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnly()
    {
        return Ok("You Are Admin => Only admins can see this");
    }

}