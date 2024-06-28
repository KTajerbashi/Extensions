using Application.Layer.Model.Base.Events;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Model.ChangeDataLog.Events;
using Application.Layer.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Layer.MediateR.Events;

public class PersonCreatedEventModel : IDomainEventHandler<PersonCreated>
{
    private readonly ILogger<PersonCreatedEventModel> _logger;
    private readonly IPersonRepository personRepository;

    public PersonCreatedEventModel(ILogger<PersonCreatedEventModel> logger, IPersonRepository personRepository)
    {
        _logger = logger;
        this.personRepository = personRepository;
    }


    public async Task Handle(PersonCreated Event)
    {
        try
        {
            Person person = new Person()
            {
                FirstName = DateTime.Now.ToString(),
                LastName = DateTime.Now.ToLongTimeString(),
            };
            personRepository.Insert(person);
            personRepository.SaveChanges();

            _logger.LogInformation("Handled {Event} in PersonCreatedEventModel", Event.GetType().Name);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
