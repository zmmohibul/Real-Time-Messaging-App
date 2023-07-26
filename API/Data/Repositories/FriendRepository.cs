using API.Dtos.Friend;
using API.Dtos.User;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class FriendRepository : IFriendRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public FriendRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FriendDto>> GetAllFriendsForUser(int userId)
    {
        var friends = await _dataContext.Friends
            .Include(f => f.FriendOne)
            .Include(f => f.FriendTwo)
            .Where(f => f.FriendOneId == userId || f.FriendTwoId == userId)
            .ToListAsync();
        
        return friends
            .Select(f => new FriendDto()
            {
                FriendsSince = f.FriendsSince,
                Friend = _mapper.Map<UserDetailsDto>(f.FriendOneId == userId ? f.FriendTwo : f.FriendOne)
            });
    }

    public async Task<Friend> GetFriend(int userId, int friendId)
    {
        return await _dataContext.Friends
            .Include(f => f.FriendOne)
            .Include(f => f.FriendTwo)
            .Where(f => (f.FriendOneId == userId && f.FriendTwoId == friendId) || (f.FriendOneId == friendId && f.FriendTwoId == userId))
            .FirstOrDefaultAsync();
    }

    public async Task<FriendDto> AddNewFriend(AppUser user, AppUser friend)
    {
        var newFriend = new Friend(user, friend);
        await _dataContext.SaveChangesAsync();

        return new FriendDto()
        {
            Friend = _mapper.Map<UserDetailsDto>(friend),
            FriendsSince = newFriend.FriendsSince
        };
    }

    public async Task<bool> AreFriends(int userId, int friendId)
    {
        var friend = await _dataContext.Friends
            .Where(f => (f.FriendOneId == userId && f.FriendTwoId == friendId) ||
                        (f.FriendOneId == friendId && f.FriendTwoId == userId))
            .FirstOrDefaultAsync();

        if (friend == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveFriend(Friend friend)
    {
        _dataContext.Remove(friend);
        return await _dataContext.SaveChangesAsync() > 0;
    }
}