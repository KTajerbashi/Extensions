using WebApi.ChangeDataLog.Models.Security;
using WebApi.ChangeDataLog.Repositories.Bases;

namespace WebApi.ChangeDataLog.Repositories.Users;

public interface IUserRepository : IBaseRepository<UserEntity, long>
{
}
