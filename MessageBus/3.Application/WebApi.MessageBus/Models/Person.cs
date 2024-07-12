using MediateR.Abstractions;

namespace WebApi.MessageBus.Models;

public class PersonCommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class PersonEvent : IDomainEvent
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

