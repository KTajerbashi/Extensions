using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.Model.ChangeDataLog;
using WebApplicationAPI.Services.Bases;

namespace WebApplicationAPI.Services.Interfaces;

public interface IPersonRepository : IBaseRepository<Person,DatabaseContext,long>
{

}
