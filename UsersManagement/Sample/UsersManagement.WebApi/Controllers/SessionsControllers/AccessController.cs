using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.SessionsControllers;

public class AccessController : BaseController
{
    public AccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }
}


