using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers;

public class AuthenticationController : BaseController
{
    private readonly ILogger<AuthenticationController> _logger;
    public AuthenticationController(IIdentityRespository identityRespository, ILogger<AuthenticationController> logger) : base(identityRespository)
    {
        _logger = logger;
    }

}


