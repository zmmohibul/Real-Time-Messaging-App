using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public AuthenticationController(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authenticationRepository.Register(registerDto);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authenticationRepository.Login(loginDto);
        return HandleResult(result);
    }
    
    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Secret()
    {
        return Ok("Secret..");
    }
}