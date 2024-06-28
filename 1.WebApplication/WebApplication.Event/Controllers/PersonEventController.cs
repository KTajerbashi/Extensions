using Application.Layer.DataAccess.ChangeDataLog;
using Application.Layer.MediateR.Commands;
using Application.Layer.MediateR.Queries;
using Application.Layer.Model.ChangeDataLog;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Controllers.Bases;

namespace WebApplicationAPI.Controllers.EventsController
{
    public class PersonEventController : BaseController
    {
        public PersonEventController(DatabaseContext context) : base(context)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePersonModel person)
        {
            await Create<CreatePersonModel, long>(person);
            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> Read(GetAllPersonModel getPerson)
        {
            await Task.CompletedTask;
            return Ok(QueryDispatcher.Execute<GetAllPersonModel, List<PersonModel>>(getPerson));
        }

        [HttpGet("ReadById")]
        public async Task<IActionResult> Read(GetPersonByIdModel getPerson)
        {
            await Task.CompletedTask;
            return Ok(QueryDispatcher.Execute<GetPersonByIdModel, PersonModel>(getPerson));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePersonModel person)
        {
            await Task.CompletedTask;
            return Ok(CommandDispatcher.Send<UpdatePersonModel, Person>(person));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeletePersonModel person)
        {
            await Task.CompletedTask;
            return Ok(CommandDispatcher.Send<DeletePersonModel, bool>(person));
        }
    }
}
