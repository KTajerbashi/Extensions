using Application.Layer.DataAccess.ChangeDataLog;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Bases;
using Application.Layer.Services.Interfaces;

namespace Application.Layer.Services.Repositories;

public class PersonRepository : BaseRepository<Person, DatabaseContext, long>, IPersonRepository
{
    public PersonRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }
}