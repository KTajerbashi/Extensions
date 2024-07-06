using Microsoft.AspNetCore.Mvc;

namespace WebApi.ChangeDataLog.Controllers;

public class RegisterController : BaseController
{
    public RegisterController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        return ;
    }

}