using Extensions.ObjectMappers.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ObjectMappers.WebApi.Models.Person;

namespace ObjectMappers.WebApi.Controllers;

/// <summary>
/// https://localhost:7097/openapi/v1.json
/// </summary>


[ApiController]
[Route("api/[controller]")]
public class PersonController : Controller
{
    private readonly IMapperAdapter _mapperAdapter;
    public PersonController(IMapperAdapter mapperAdapter)
    {
        _mapperAdapter = mapperAdapter;
    }
    /// <summary>
    /// https://localhost:7097/api/Person
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        var entity = new PersonEntity()
        {
            Id = 1,
            BirthDate = DateTime.Now,
            Email = "User1@mail.com",
            Name = "Test",
            Fmaily = "",
            Password = "",
            Key = Guid.NewGuid(),
        };
        var dto = _mapperAdapter.Map<PersonEntity,PersonDto>(entity);

        return Ok("Ok");
    }
}
