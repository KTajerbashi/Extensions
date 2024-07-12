using MediateR.Abstractions;

namespace WebApi.MessageBus.Models;

public class PersonCommand:ICommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class PersonEvent : IDomainEvent
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

