using API.Dtos.User;

namespace API.Dtos.FriendRequest;

public class FriendRequestDto
{
    public int Id { get; set; }
    
    public UserDetailsDto RequestFrom { get; set; }
    
    public DateTime RequestDate { get; set; }
}