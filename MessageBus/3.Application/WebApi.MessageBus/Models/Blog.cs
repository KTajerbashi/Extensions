using MediateR.Abstractions;

namespace WebApi.MessageBus.Models;

public class BlogCommand:ICommand
{
    public string BusinessId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
}
public class BlogCreated : IDomainEvent
{
    public string BusinessId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public BlogCreated(string businessId, string title, string description)
    {
        BusinessId = businessId;
        Title = title;
        Description = description;
    }
}
