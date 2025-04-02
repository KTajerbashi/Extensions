using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.TokensControllers;

public class TokenAccessController : BaseController
{
    public TokenAccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }
}


