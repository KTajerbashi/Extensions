namespace ObjectMappers.WebApi.Models.Person;

public class PersonView
{
    public string Name { get; set; }
    public string Fmaily { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string BirthDateShow { get => BirthDate.ToShortDateString(); }

}
