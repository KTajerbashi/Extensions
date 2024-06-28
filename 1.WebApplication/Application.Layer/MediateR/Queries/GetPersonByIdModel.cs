using Application.Layer.Model.Base.Queries;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Interfaces;

namespace Application.Layer.MediateR.Queries;
public class GetPersonByIdModel : IQuery<PersonModel>
{
    public long Id { get; set; }
}
public class GetPersonByIdHandler : QueryHandler<GetPersonByIdModel, PersonModel>
{
    private readonly IPersonRepository personRepository;

    public GetPersonByIdHandler(IPersonRepository personRepository)
    {
        this.personRepository = personRepository;
    }

    public override Task<QueryResult<PersonModel>> Handle(GetPersonByIdModel query)
    {
        var result = personRepository.Get(new Person(query.Id,"",""));
        return ResultAsync(new PersonModel
        {
            Id = result.Id,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Email = result.Email,
        });
    }
}