using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.TokensControllers;

[Authorize] // This applies to all actions in the controller
public class TokenSecureController : BaseController
{
    public TokenSecureController(IIdentityRespository identityRespository) : base(identityRespository)
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