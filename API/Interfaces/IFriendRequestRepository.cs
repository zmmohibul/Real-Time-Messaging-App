using API.Dtos.FriendRequest;
using API.Entities;

namespace API.Interfaces;

public interface IFriendRequestRepository
{
    Task<IEnumerable<FriendRequestDto>> GetAllRequestsForUser(int userId);
    Task<FriendRequest> GetRequestsById(int requestId);
    Task<bool> CreateRequest(AppUser requestFromUser, AppUser requestToUser);
    Task<bool> DeleteFriendRequestById(int requestId);

    Task<bool> RequestExist(int requestFromUserId, int requestToUserId);
}