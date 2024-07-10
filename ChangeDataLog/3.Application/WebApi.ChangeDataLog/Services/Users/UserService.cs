using WebApi.ChangeDataLog.Database;
using WebApi.ChangeDataLog.Models.Security;
using WebApi.ChangeDataLog.Repositories.Users;
using WebApi.ChangeDataLog.Services.Bases;

namespace WebApi.ChangeDataLog.Services.Users;

public class UserService : BaseRepository<UserEntity, ApplicationContext, long>, IUserRepository
{
    public UserService(ApplicationContext context) : base(context)
    {
    }
}
