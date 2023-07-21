using API.Dtos;
using API.Dtos.Authentication;
using API.Dtos.User;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly DataContext _dataContext;
    private readonly ITokenService _tokenService;

    public AuthenticationRepository(DataContext dataContext, ITokenService tokenService)
    {
        _dataContext = dataContext;
        _tokenService = tokenService;
    }

    public async Task<Result<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName))
        {
            return Result<UserDto>.BadRequestResult("UserName is taken");
        }

        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
        };
        
        Password.HashPassword(user, registerDto.Password);

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        var userDto = new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        
        return Result<UserDto>.OkResult(userDto);
    }

    public async Task<Result<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(user =>
            user.UserName.Equals(loginDto.UserName.ToLower()));
        
        if (user == null)
        {
            return Result<UserDto>.BadRequestResult("Invalid userName");
        }

        if (!Password.PasswordValid(user, loginDto.Password))
        {
            return Result<UserDto>.BadRequestResult("Invalid Password");
        }
        
        var userDto = new UserDto()
        {
            Id = user.Id,
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        
        return Result<UserDto>.OkResult(userDto);
    }

    private async Task<bool> UserExists(string username)
    {
        return await _dataContext.Users.AnyAsync(user => user.UserName == username.ToLower());
    }
}