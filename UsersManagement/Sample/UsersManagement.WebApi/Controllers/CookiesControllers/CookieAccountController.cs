using Extensions.UsersManagement.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersManagement.WebApi.Interfaces;
using UsersManagement.WebApi.Models.DTOs;
using UsersManagement.WebApi.Models.Entities;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

public class CookieAccountController : BaseController
{
    public CookieAccountController(IIdentityRespository identityRespository) : base(identityRespository)
    {
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

    [HttpPost("RegisterList")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterList()
    {
        var dataList = new List<RegisterModel>();
        dataList.Add(new RegisterModel() { Username = "Tajerbashi", Email = "tajerbashi@mail.com", Password = "@Kamran#100", PhoneNumber = "09011001230", FirstName = "Kamran", LastName = "Tajerbashi", DisplayName = "Tajer-K", FullName = "Kamran Tajerbashi", IsAdmin = true });
        dataList.Add(new RegisterModel() { Username = "Kaihan", Email = "yousefzay@mail.com", Password = "@Kaihan#100", PhoneNumber = "09181521560", FirstName = "Kaihan", LastName = "Yousefzay", DisplayName = "Yousef-K", FullName = "Kaihan Yousefzay", IsAdmin = false });

        foreach (var item in dataList)
        {
            await Register(item);
        }

        return Ok("OK");
    }
}


