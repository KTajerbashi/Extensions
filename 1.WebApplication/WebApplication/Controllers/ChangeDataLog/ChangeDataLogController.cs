using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;
using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.Model.ChangeDataLog;

namespace WebApplicationAPI.Controllers.ChangeDataLog;

public class ChangeDataLogController : BaseController
{
    public ChangeDataLogController(DatabaseContext context) : base(context)
    {
    }

    [HttpPost]
    public IActionResult CreatePerson(Person person)
    {
        var items = context.People.ToList();
        foreach (var item in items)
        {
            item.Name = item.Name + " " + DateTime.Now.Ticks;
        }
        context.Add(person);
        context.SaveChanges();
        return Ok(person);
    }
    [HttpPut]
    public IActionResult UpdatePerson(Person person)
    {
        var items = context.People.Where(item => item.Id.Equals(person.Id)).Single();
        items.FullName = person.FullName;
        items.Name = person.Name;
        context.Update(items);
        context.SaveChanges();
        return Ok(person);
    }
    [HttpDelete]
    public IActionResult DeletePerson(Person person)
    {
        var items = context.People.Where(item =>
        item.Id.Equals(person.Id) ||
        item.Name.Equals(person.Name) ||
        item.FullName.Equals(person.FullName)
        ).FirstOrDefault();
        context.Remove(items);
        context.SaveChanges();
        return Ok(context.People.ToList());
    }
    [HttpGet("Read")]
    public IActionResult GetPerson(Person person)
    {
        var items = context.People.Where(item => 
        item.Id.Equals(person.Id) ||
        item.Name.Equals(person.Name) ||
        item.FullName.Equals(person.FullName) 
        ).FirstOrDefault();
        return Ok(items);
    }
    [HttpGet("ReadAll")]
    public IActionResult GetPeople()
    {
        return Ok(context.People.ToList());
    }
}
