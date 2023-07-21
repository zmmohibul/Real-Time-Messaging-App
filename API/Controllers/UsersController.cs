using API.Dtos.User;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] QueryParameters queryParameters)
    {
        return HandleResult(await _userRepository.GetAllUsersAsync(queryParameters));
    }
    
    [HttpGet("{username}")]
    public async Task<IActionResult> GetAllUsers(string username)
    {
        return HandleResult(await _userRepository.GetUserByUserNameAsync(username));
    }
    
    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUser(string username, UpdateUserDto updateUserDto)
    {
        return HandleResult(await _userRepository.UpdateUserAsync(username, updateUserDto));
    }
    
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        return HandleResult(await _userRepository.DeleteUserAsync(username));
    }
    
}