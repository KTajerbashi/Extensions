using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.TokensControllers;



[ApiController]
[Route("Tokens/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IIdentityRespository Repository;

    protected BaseController(IIdentityRespository identityRespository)
    {
        Repository = identityRespository;
    }
}
