﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serializers.WebApi.Models;

namespace Serializers.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{


    public override OkObjectResult Ok([ActionResultObjectValue] object? value)
    {
        return base.Ok(ResponseApi.Success(value));
    }
}
