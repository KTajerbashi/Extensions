using WebApplicationAPI.Model.Base;

namespace WebApplicationAPI.Model.ChangeDataLog.Events
{
    public record PersonCreated(long Id, string FirstName, string LastName,string Email) : IDomainEvent;
    public record PersonNameChanged(long Id, string FirstName) : IDomainEvent;
    public record PersonSendEmail(string FirstName) : IDomainEvent;
}
