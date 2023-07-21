using API.Dtos;
using API.Dtos.User;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<Result<PaginatedList<UserDetailsDto>>> GetAllUsersAsync(QueryParameters queryParameters);
    Task<Result<UserDetailsDto>> GetUserByUserNameAsync(string userName);
    Task<Result<UserDetailsDto>> UpdateUserAsync(string userName, UpdateUserDto updateUserDto);
    Task<Result<bool>> DeleteUserAsync(string userName);
    

}