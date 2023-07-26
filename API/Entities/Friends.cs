namespace API.Entities;

public class Friend
{
    public int Id { get; set; }
    
    public int FriendOneId { get; set; }
    public AppUser FriendOne { get; set; }

    public int FriendTwoId { get; set; }
    public AppUser FriendTwo { get; set; }

    public DateTime FriendsSince { get; set; } = DateTime.UtcNow;

    public Friend() {}

    public Friend(AppUser friendOne, AppUser friendTwo)
    {
        FriendOne = friendOne;
        FriendTwo = friendTwo;
    }
}