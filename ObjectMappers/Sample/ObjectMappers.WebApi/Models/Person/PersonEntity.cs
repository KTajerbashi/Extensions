namespace ObjectMappers.WebApi.Models.Person;

public class PersonEntity
{
    public long Id { get; set; }
    public Guid Key { get; set; }
    public string Name { get; set; }
    public string Fmaily { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
}
