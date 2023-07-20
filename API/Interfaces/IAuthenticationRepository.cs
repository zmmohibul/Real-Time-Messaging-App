using API.Dtos;
using API.Helpers;

namespace API.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result<UserDto>> Register(RegisterDto registerDto);
    Task<Result<UserDto>> Login(LoginDto loginDto);
}