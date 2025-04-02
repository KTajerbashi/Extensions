using Extensions.UsersManagement.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.DTOs;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

public class CookieAuthController : BaseController
{
    private readonly ILogger<CookieAuthController> _logger;
    public CookieAuthController(IIdentityRespository identityRespository, ILogger<CookieAuthController> logger) : base(identityRespository)
    {
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await Repository.SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok();
        }

        return Unauthorized();
    }
    [HttpPost("loginAsId")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> loginAsId(long id)
    {

        var user = await Repository.UserManager.FindByIdAsync($"{id}");
        if (user is null)
            return Unauthorized();

        var roles  = await Repository.UserManager.GetRolesAsync(user);
        if (user is null)
            return NotFound();
        await Repository.SignInManager.SignOutAsync();

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,$"{user.Id}"),
            new Claim(ClaimTypes.Name,user.DisplayName),
            new Claim(ClaimTypes.Email,user.Email),
        };
        if (roles.Any(role => role.ToLower().Equals("admin")))
        {
            claims.Add(new Claim("permission", "content.edit"));
        }
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

        await Repository.UserManager.AddClaimsAsync(user, claims);

        await Repository.SignInManager.SignInAsync(user, true);
        //await Repository.SignInManager.SignInAsync(user, true);
        return Ok();
    }
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.GetClaim(ClaimTypes.NameIdentifier);
        var user = await Repository.UserManager.FindByIdAsync(userId.Value);
        await Repository.UserManager.RemoveClaimsAsync(user, User.Claims);
        await Repository.SignInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("IsAuthenticated")]
    public IActionResult IsAuthenticated()
    {
        return Ok(User.Identity.IsAuthenticated);
    }

    [HttpGet("Claims")]
    public IActionResult Claims()
    {
        var response = User.Claims.Select(item => new {Key = item.Type,Value = item.Value}).ToList();
        return Ok(response);
    }

    [HttpGet("GetClaim/{key}")]
    public IActionResult GetClaim(string key)
    {
        return Ok(User.GetClaim(key));
    }

    [HttpGet("UserClaims")]
    public async Task<IActionResult> UserClaims()
    {
        var userId = User.GetClaim("UserId");
        var user = await Repository.UserManager.FindByIdAsync(userId.Value);
        return Ok(Repository.UserManager.GetClaimsAsync(user!));
    }
}


