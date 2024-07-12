using Microsoft.AspNetCore.Mvc;
using WebApi.DependencyInjection.Services.Customer;

namespace WebApi.DependencyInjection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomController : ControllerBase
{
    private readonly IScopeServices<string> _scopeService;

    public CustomController(IScopeServices<string> scopeService)
    {
        _scopeService = scopeService;
    }


    [HttpPost()]
    public async Task<IActionResult> Create(string value)
    {
        _scopeService.Create(value);
        return Ok(_scopeService.Get());
    }

    [HttpPut()]
    public async Task<IActionResult> Update(string value, int index)
    {
        _scopeService.Update(value, index);
        return Ok(_scopeService.Get());
    }

    [HttpDelete()]
    public async Task<IActionResult> Delete(int index)
    {
        return Ok(_scopeService.Get());
    }

    [HttpGet()]
    public async Task<IActionResult> Get()
    {
        return Ok(_scopeService.Get());
    }

    [HttpGet("{index}")]
    public async Task<IActionResult> Get(int index)
    {
        return Ok(_scopeService.GetIndex(index));
    }
}
