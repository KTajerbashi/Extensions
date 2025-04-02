using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

[Authorize] // This applies to all actions in the controller
public class CookieSecureController : BaseController
{
    public CookieSecureController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("This is a secure endpoint");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("AdminRole")]
    public IActionResult AdminRole()
    {
        return Ok("You Are Admin => Only admins can see this");
    }

    [Authorize(Roles = "User")]
    [HttpGet("UserRole")]
    public IActionResult UserRole()
    {
        return Ok("You Are User => Only user can see this");
    }


    [Authorize(Roles = "User,Admin")]
    [HttpGet("AdminAndUser")]
    public IActionResult AdminAndUser()
    {
        return Ok("You Are Admin and User => Only admin & user can see this");
    }


    [Authorize(Policy = "RequireAdmin")]
    [HttpGet("AdminPolicy")]
    public IActionResult AdminPolicy()
    {
        return Ok("You Are Admin => Only admins can see this");
    }

    [Authorize(Policy = "EditContent")]
    [HttpGet("CanEdit")]
    public IActionResult CanEdit()
    {
        return Ok("You Are Can Edit => Content");
    }

}