using Microsoft.AspNetCore.Mvc;
using Serializers.WebApi.Modules;

namespace Serializers.WebApi.Controllers;

public class VariablesController : BaseController
{
    public long UserId { get; set; }
    public VariablesController()
    {
        Console.WriteLine($"VariablesController Create New : Request Count => {StaticMemory.Counter}");
        StaticMemory.Counter++;
    }

    [HttpGet("GetCurrentStatic")]
    public IActionResult GetCurrentStatic()
    {

        return Ok(new
        {
            UserId = StaticMemory.UserId,
            Hash = StaticMemory.UserId.GetType().Assembly.Location
        });
    }
    [HttpGet("GetNewStatic")]
    public IActionResult GetNewStatic()
    {
        return Ok(new
        {
            UserId = UserId,
            Hash = UserId.GetType().Assembly.Location
        });
    }
    [HttpGet("SetStaticNumber")]
    public IActionResult SetStaticNumber(int a)
    {
        StaticMemory.UserId = a;
        return Ok("SetStaticNumber");
    }
    [HttpGet("SetLocalNumber")]
    public IActionResult SetLocalNumber(int a)
    {
        UserId = a;
        return Ok("SetLocalNumber");
    }
}
