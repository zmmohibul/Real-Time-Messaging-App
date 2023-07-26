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

        return await GetUsersUsingQuerySourceProcessingQueryParameters(query, queryParameters);
    }

    public async Task<Result<PaginatedList<UserDetailsDto>>> GetAllUsersWhoAreNotFriendOfCurrentUserAsync(int currUserId, QueryParameters queryParameters)
    {
        var friendsOfCurrentUser = await _dataContext.Friends
            .AsNoTracking()
            .Where(f => f.FriendOneId == currUserId || f.FriendTwoId == currUserId)
            .Select(f => f.FriendOneId == currUserId ? f.FriendTwoId : f.FriendOneId)
            .ToListAsync();

        var currentUsersRequests = await _dataContext.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.RequestToId == currUserId || fr.RequestFromId == currUserId)
            .Select(fr => fr.RequestToId == currUserId ? fr.RequestFromId : fr.RequestToId)
            .ToListAsync();

        var query = _dataContext.Users
            .AsNoTracking()
            .Where(user => !friendsOfCurrentUser.Contains(user.Id) && !currentUsersRequests.Contains(user.Id));

        return await GetUsersUsingQuerySourceProcessingQueryParameters(query, queryParameters);
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

    public async Task<Result<PaginatedList<UserDetailsDto>>> GetUsersUsingQuerySourceProcessingQueryParameters(
        IQueryable<AppUser> query, QueryParameters queryParameters)
    {
        if (!string.IsNullOrEmpty(queryParameters.SearchTerm))
        {
            query = query.Where(user => user.UserName.Contains(queryParameters.SearchTerm.ToLower()));
        }

        query = query.OrderBy(user => user.UserName);
        
        var projectedQuery = query.ProjectTo<UserDetailsDto>(_mapper.ConfigurationProvider);
        
        var data = await PaginatedList<UserDetailsDto>.CreatePaginatedListAsync(projectedQuery,
            queryParameters.PageNumber, queryParameters.PageSize);

        return Result<PaginatedList<UserDetailsDto>>.OkResult(data);
    }
}