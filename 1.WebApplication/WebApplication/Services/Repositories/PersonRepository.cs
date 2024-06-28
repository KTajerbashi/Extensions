using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.Model.ChangeDataLog;
using WebApplicationAPI.Services.Bases;
using WebApplicationAPI.Services.Interfaces;

namespace WebApplicationAPI.Services.Repositories;

public class PersonRepository : BaseRepository<Person, DatabaseContext, long>, IPersonRepository
{
    public PersonRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}