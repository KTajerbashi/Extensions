using Application.Layer.DataAccess.ChangeDataLog;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Bases;

namespace Application.Layer.Services.Interfaces;

public interface IPersonRepository : IBaseRepository<Person, DatabaseContext, long>
{

}
