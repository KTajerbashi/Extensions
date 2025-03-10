using Microsoft.AspNetCore.Mvc;
using Serializers.WebApi.Modules;

namespace Serializers.WebApi.Controllers;
public class DependencyController : BaseController
{
    private readonly IIDSingleton _singletonA;
    private readonly IIDSingleton _singletonB;
    private readonly IIDScoped _scopedA;
    private readonly IIDScoped _scopedB;
    private readonly IIDTransient _transientA;
    private readonly IIDTransient _transientB;

    public DependencyController(IIDSingleton singletonA, IIDSingleton singletonB, IIDScoped scopedA, IIDScoped scopedB, IIDTransient transientA, IIDTransient transientB)
    {
        _singletonA = singletonA;
        _singletonB = singletonB;
        _scopedA = scopedA;
        _scopedB = scopedB;
        _transientA = transientA;
        _transientB = transientB;
    }

    [HttpGet("ShowLifeTime")]
    public IActionResult SendCode()
    {
        Console.WriteLine($"Single 1 : {_singletonA.Value}");
        Console.WriteLine($"Single 2 : {_singletonB.Value}");
        Console.WriteLine("=========================================");
        Console.WriteLine($"Scoped 1: {_scopedA.Value}");
        Console.WriteLine($"Scoped 2: {_scopedB.Value}");
        Console.WriteLine("=========================================");
        Console.WriteLine($"Transient 1: {_transientA.Value}");
        Console.WriteLine($"Transient 2: {_transientB.Value}");
        Console.WriteLine("+++++++++++++++++++++++++++++++++++\n");
        return Ok();
    }

}
