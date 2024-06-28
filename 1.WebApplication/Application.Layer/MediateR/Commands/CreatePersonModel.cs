using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Interfaces;

namespace Application.Layer.MediateR.Commands;

public class CreatePersonModel : ICommand<long>
{
    #region Properties
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion

}

public class CreatePersonHandler : CommandHandler<CreatePersonModel, long>
{
    private readonly IPersonRepository _personRepository;
    public CreatePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    public override async Task<CommandResult<long>> Handle(CreatePersonModel command)
    {
        var entity = new Person(command.Id,command.FirstName,command.LastName);
        entity.ChangeFirstName(command.FirstName);
        _personRepository.Insert(entity);
        await _personRepository.SaveChangesAsync();
        return await OkAsync(entity.Id);
    }
}
