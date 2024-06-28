using Application.Layer.Model.Base;
using Application.Layer.Model.ChangeDataLog.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Layer.Model.ChangeDataLog;

[Table("People",Schema ="Model")]
public class Person : AggregateRoot<long>
{
    #region Properties
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion

    public Person(int id,
        string firstName,
        string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = $"{firstName}_{lastName}@mail.com";
        AddEvent(new PersonCreated(id, firstName, lastName, Email));
    }
    public void ChangeFirstName(string firstName)
    {
        FirstName = firstName;
        AddEvent(new PersonNameChanged(Id, firstName));
    }

}
public class PersonEventBase : AggregateRoot<long>
{
    #region Properties
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    #endregion
    public PersonEventBase(int id, string firstName, string lastName, string email)
    {
        Email = $"{firstName}_{lastName}@mail.com";
        Apply(new PersonCreated(id, firstName, lastName, Email));
    }
    private void On(PersonCreated personCreated)
    {
        Id = personCreated.Id;
        FirstName = personCreated.FirstName;
        LastName = personCreated.LastName;
        Email = personCreated.Email;
    }
}
