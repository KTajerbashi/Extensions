using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.CookiesControllers;

[ApiController]
[Route("Cookies/[controller]")]
[EndpointGroupName("Cookies")]
public abstract class BaseController : ControllerBase
{
    protected readonly IIdentityRespository Repository;

    protected BaseController(IIdentityRespository identityRespository)
    {
        Repository = identityRespository;
    }
}
