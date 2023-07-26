using API.Dtos.Friend;
using API.Entities;

namespace API.Interfaces;

public interface IFriendRepository
{
    Task<IEnumerable<FriendDto>> GetAllFriendsForUser(int userId);
    Task<FriendDto> AddNewFriend(AppUser user, AppUser friend);
    Task<bool> AreFriends(int userId, int friendId);
    Task<bool> RemoveFriend(Friend friend);
}