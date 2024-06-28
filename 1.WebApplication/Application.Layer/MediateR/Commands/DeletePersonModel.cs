using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Interfaces;
using Application.Layer.Services.Repositories;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Application.Layer.MediateR.Commands;

public class DeletePersonModel : ICommand<bool>
{
    #region Properties
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion
}
public class DeletePersonHandler : CommandHandler<DeletePersonModel, bool>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public override async Task<CommandResult<bool>> Handle(DeletePersonModel command)
    {
        var entity = new Person(command.Id,command.FirstName,command.LastName);
        _personRepository.Delete(entity);
        _personRepository.SaveChanges();
        return await OkAsync(true);
    }
}