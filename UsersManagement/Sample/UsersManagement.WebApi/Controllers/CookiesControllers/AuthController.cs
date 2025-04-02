﻿using Extensions.UsersManagement.Extensions;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

public class AuthController : BaseController
{
    private readonly ILogger<AuthController> _logger;
    public AuthController(IIdentityRespository identityRespository, ILogger<AuthController> logger) : base(identityRespository)
    {
        _logger = logger;
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


