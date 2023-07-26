using API.Dtos.User;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] QueryParameters queryParameters)
    {
        return HandleResult(await _userRepository.GetAllUsersWhoAreNotFriendOfCurrentUserAsync(User.GetUserId(), queryParameters));
    }
    
    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserByUserName(string username)
    {
        var user = await _userRepository.GetUserByUserNameAsync(username);
        
        return Ok(_mapper.Map<UserDetailsDto>(user));
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