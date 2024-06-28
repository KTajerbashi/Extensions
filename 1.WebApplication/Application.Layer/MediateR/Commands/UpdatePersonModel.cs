using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Interfaces;

namespace Application.Layer.MediateR.Commands;

public class UpdatePersonModel : ICommand<Person>
{
    #region Properties
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion

}
public class UpdatePersonHandler : CommandHandler<UpdatePersonModel, Person>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public override async Task<CommandResult<Person>> Handle(UpdatePersonModel command)
    {
        var entity = new Person(command.Id,command.FirstName,command.LastName);
        _personRepository.Update(entity);
        _personRepository.SaveChanges();
        return await OkAsync(entity);
    }
}