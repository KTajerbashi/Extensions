﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers;

[Authorize] // This applies to all actions in the controller
public class SecureController : BaseController
{
    public SecureController(IIdentityRespository identityRespository) : base(identityRespository)
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
        return Ok("Only admins can see this");
    }

}