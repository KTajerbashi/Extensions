using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.DTOs;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Controllers.TokensControllers;

public class AccountController : BaseController
{
    public AccountController(IIdentityRespository identityRespository) : base(identityRespository)
    {
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
            return NotFound();
        await Repository.SignInManager.SignOutAsync();
        await Repository.SignInManager.SignInAsync(user, true);
        return Ok();
    }
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await Repository.SignInManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var response= new IdentityResult();
        var userEntity = new ApplicationUser()
        {
            UserName = model.Username,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            FirstName = model.FirstName,
            LastName = model.LastName,
            DisplayName = model.DisplayName,
        };
        response = await Repository.UserManager.CreateAsync(userEntity, model.Password);
        var adminRole = new ApplicationRole("Admin");
        var userRole = new ApplicationRole("User");

        if (!(await Repository.RoleManager.RoleExistsAsync(adminRole.Name)))
            response = await Repository.RoleManager.CreateAsync(adminRole);
        if (!(await Repository.RoleManager.RoleExistsAsync(userRole.Name)))
            response = await Repository.RoleManager.CreateAsync(userRole);

        if (model.IsAdmin)
            response = await Repository.UserManager.AddToRoleAsync(userEntity, adminRole.Name);
        else
            response = await Repository.UserManager.AddToRoleAsync(userEntity, adminRole.Name);

        return Ok(response);
    }

}


