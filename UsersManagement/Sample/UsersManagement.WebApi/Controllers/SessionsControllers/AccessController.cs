using UsersManagement.WebApi.Interfaces;

namespace UsersManagement.WebApi.Controllers.SessionsControllers;

public class SessionAccessController : BaseController
{
    public SessionAccessController(IIdentityRespository identityRespository) : base(identityRespository)
    {
    }
}


