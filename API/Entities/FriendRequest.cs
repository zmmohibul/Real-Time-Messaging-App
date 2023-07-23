namespace API.Entities;

public class FriendRequest
{
    public int Id { get; set; }

    public int RequestFromId { get; set; }
    public AppUser RequestFrom { get; set; }

    public int RequestToId { get; set; }
    public AppUser RequestTo { get; set; }
    

    public DateTime RequestDate { get; set; } = DateTime.UtcNow;

    public FriendRequest() {}

    public FriendRequest(AppUser requestFrom, AppUser requestTo)
    {
        RequestFrom = requestFrom;
        RequestTo = requestTo;
    }
}