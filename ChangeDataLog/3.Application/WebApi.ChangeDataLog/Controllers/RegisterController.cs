using Microsoft.AspNetCore.Mvc;
using WebApi.ChangeDataLog.Models.Security;
using WebApi.ChangeDataLog.Repositories.Users;

namespace WebApi.ChangeDataLog.Controllers;

public class UserController : BaseController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserEntity entity) => Ok(await _userRepository.CreateAsync(entity));

    [HttpPut]
    public async Task<IActionResult> Update(UserEntity entity) => Ok(await _userRepository.UpdateAsync(entity));

    [HttpDelete]
    public async Task<IActionResult> Delete(UserEntity entity) => Ok(await _userRepository.DeleteAsync(entity.Id));

    [HttpGet("GetById")]
    public async Task<IActionResult> Get(long Id) => Ok(await _userRepository.GetByIdAsync(Id));

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _userRepository.GetAsync());

}