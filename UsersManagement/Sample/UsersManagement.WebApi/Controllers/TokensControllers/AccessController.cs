using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.TokensControllers;

public class AccessController : BaseController
{
    public AccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }
}


