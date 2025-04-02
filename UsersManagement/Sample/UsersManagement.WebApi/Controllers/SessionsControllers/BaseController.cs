using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.SessionsControllers;


[ApiController]
[Route("Sessions/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IIdentityRespository Repository;

    protected BaseController(IIdentityRespository identityRespository)
    {
        Repository = identityRespository;
    }
}
