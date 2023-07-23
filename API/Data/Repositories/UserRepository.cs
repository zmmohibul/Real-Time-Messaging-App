using API.Dtos.User;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<Result<PaginatedList<UserDetailsDto>>> GetAllUsersAsync(QueryParameters queryParameters)
    {
        var query = _dataContext.Users.AsNoTracking();

        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            query = query.Where(user => user.UserName.Contains(queryParameters.SearchTerm.ToLower()));
        }

        var projectedQuery = query.ProjectTo<UserDetailsDto>(_mapper.ConfigurationProvider);

        var data = await PaginatedList<UserDetailsDto>.CreatePaginatedListAsync(projectedQuery,
            queryParameters.PageNumber, queryParameters.PageSize);

        return Result<PaginatedList<UserDetailsDto>>.OkResult(data);
    }

    public async Task<AppUser> GetUserByUserNameAsync(string userName)
    {
        return await _dataContext.Users
            .SingleOrDefaultAsync(user => user.UserName.Equals(userName.ToLower()));
    }

    public async Task<AppUser> GetUserByUserIdAsync(int userId)
    {
        return await _dataContext.Users.FindAsync(userId);
    }

    public async Task<Result<UserDetailsDto>> UpdateUserAsync(string userName, UpdateUserDto updateUserDto)
    {
        var user = await _dataContext.Users
            .SingleOrDefaultAsync(user => user.UserName.Equals(userName));

        if (user == null)
        {
            return Result<UserDetailsDto>.NotFoundResult($"User {userName} not found");
        }

        _mapper.Map(updateUserDto, user);

        await _dataContext.SaveChangesAsync();
        
        return Result<UserDetailsDto>.OkResult(_mapper.Map<UserDetailsDto>(user));
    }

    public async Task<Result<bool>> DeleteUserAsync(string userName)
    {
        var user = await _dataContext.Users
            .SingleOrDefaultAsync(user => user.UserName.Equals(userName));

        if (user == null)
        {
            return Result<bool>.NotFoundResult($"User {userName} not found");
        }

        _dataContext.Remove(user);

        if (!(await _dataContext.SaveChangesAsync() > 0))
        {
            return Result<bool>.BadRequestResult("Could not delete user");
        }
        
        return Result<bool>.NoContentResult();
    }
}