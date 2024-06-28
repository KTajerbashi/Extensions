using Application.Layer.DataAccess.ChangeDataLog;
using Application.Layer.Model.ChangeDataLog;
using Application.Layer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;

namespace WebApplicationAPI.Controllers.ChangeDataLog;

public class ChangeDataLogController : BaseController
{
    private readonly IPersonRepository personRepository;
    public ChangeDataLogController(DatabaseContext context, IPersonRepository personRepository) : base(context)
    {
        this.personRepository = personRepository;
    }

    [HttpPost]
    public IActionResult Create (Person person)
    {
        var items = context.People.ToList();
        foreach (var item in items)
        {
            item.FirstName = item.FirstName + " " + DateTime.Now.Ticks;
        }
        personRepository.Insert(person);
        personRepository.SaveChanges();
        return Ok(person);
    }
    [HttpPut]
    public IActionResult Update(Person person)
    {
        var items = context.People.Where(item => item.Id.Equals(person.Id)).Single();
        items.FirstName = person.FirstName;
        items.LastName = person.LastName;
        items.Email = person.Email;
        personRepository.Update(items);
        personRepository.SaveChanges();
        return Ok(person);
    }
    [HttpDelete]
    public IActionResult Delete(Person person)
    {
        personRepository.Delete(personRepository.Get(person));
        personRepository.SaveChanges();
        return Ok(context.People.ToList());
    }
    [HttpGet("Read")]
    public IActionResult Get(Person person)
    {
        return Ok(personRepository.Get(person));
    }
    [HttpGet("ReadAll")]
    public IActionResult Get()
    {
        return Ok(personRepository.GetAll());
    }
}
