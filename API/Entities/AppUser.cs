namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
    public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();
}