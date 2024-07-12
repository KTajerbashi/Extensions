using Microsoft.AspNetCore.Mvc;
using WebApi.DependencyInjection.Services.Customer;
using WebApi.DependencyInjection.Services.Scope;
using WebApi.DependencyInjection.Services.Singleton;
using WebApi.DependencyInjection.Services.Transient;

namespace WebApi.DependencyInjection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuidGeneratorController : ControllerBase
{
    private readonly IGetGuidSingletonService _getRandomNumberSingletonService;
    public GuidGeneratorController(IGetGuidSingletonService getRandomNumberSingletonService)
    {
        _getRandomNumberSingletonService = getRandomNumberSingletonService;
    }

    [HttpGet("GetRandomNumberTransient")]
    public async Task<IActionResult> GetRandomNumberTransient([FromServices] IGetGuidTransientService service1,
                                                              [FromServices] IGetGuidTransientService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}", service1.Execute(), service2.Execute()));

    [HttpGet("GetRandomNumberScope")]
    public async Task<IActionResult> GetRandomNumberScope([FromServices] IGetGuidScopeService service1,
                                                          [FromServices] IGetGuidScopeService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}",
            service1.Execute(),
            service2.Execute()));

    [HttpGet("GetRandomNumberSingleton")]
    public async Task<IActionResult> GetRandomNumberSingleton([FromServices] IGetGuidSingletonService service1,
                                                               [FromServices] IGetGuidSingletonService service2)
        => Ok(string.Format("1 : {0} , 2 : {1}", service1.Execute(), service2.Execute()));


    [HttpGet("GetRandomNumberStringScope")]
    public async Task<IActionResult> GetRandomNumberStringScope([FromServices] IGetStringScopeService service1,
                                                             [FromServices] IGetStringScopeService service2)
      => Ok(string.Format("1 : {0} , 2 : {1}", service1.Execute("GetRandomNumberStringScope1"), service2.Execute("GetRandomNumberStringScope2")));

}