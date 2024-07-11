using Microsoft.AspNetCore.Mvc;
using WebApi.DependencyInjection.Services;
using WebApi.DependencyInjection.Services.Customer;

namespace WebApi.DependencyInjection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuidGeneratorController : ControllerBase
{
    private readonly IGetGuidSingletonService _getRandomNumberSingletonService;
    private readonly ICustomerServices<string> _customerServices;
    public GuidGeneratorController(IGetGuidSingletonService getRandomNumberSingletonService, ICustomerServices<string> customerServices)
    {
        _getRandomNumberSingletonService = getRandomNumberSingletonService;
        _customerServices = customerServices;
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



    [HttpPost("CustomCreate")]
    public async Task<IActionResult> Create(string value)
    {
        _customerServices.Create(value);
        return Ok(_customerServices.Get());
    }

    [HttpPut("CustomCreate")]
    public async Task<IActionResult> Update(string value,int index)
    {
        _customerServices.Update(value,index);
        return Ok(_customerServices.Get());
    }

    [HttpDelete("CustomCreate")]
    public async Task<IActionResult> Delete(int index)
    {
        return Ok(_customerServices.Get());
    }

    [HttpGet("CustomCreate")]
    public async Task<IActionResult> Get(string value)
    {
        return Ok(_customerServices.Get());
    }
}