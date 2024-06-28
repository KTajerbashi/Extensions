using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;
using WebApplicationAPI.DataAccess.ChangeDataLog;
using WebApplicationAPI.Model.ChangeDataLog;
using WebApplicationAPI.Services.Interfaces;
using WebApplicationAPI.Services.Repositories;

namespace WebApplicationAPI.Controllers.EventsController
{
    public class PersonEventController : BaseController
    {
        private readonly IPersonRepository personRepository;
        public PersonEventController(DatabaseContext context, IPersonRepository personRepository) : base(context)
        {
            this.personRepository = personRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Person person)
        {
            Person entity = new Person(person.Id,person.FirstName,person.LastName);
            entity.ChangeFirstName(person.FirstName);
            personRepository.Insert(entity);
            await personRepository.SaveChangesAsync();
            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            await Task.CompletedTask;
            return Ok(personRepository.GetAll());
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> Read(Person person)
        {
            await Task.CompletedTask;
            return Ok(personRepository.Get(person));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Person person)
        {
            await Task.CompletedTask;
            return Ok(personRepository.Update(person));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Person person)
        {
            await Task.CompletedTask;
            return Ok(personRepository.Delete(person));
        }
    }
}
