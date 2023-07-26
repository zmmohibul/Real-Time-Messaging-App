using API.Dtos.User;

namespace API.Dtos.Friend;

public class FriendDto
{
    public UserDetailsDto Friend { get; set; }
    public DateTime FriendsSince { get; set; }
}