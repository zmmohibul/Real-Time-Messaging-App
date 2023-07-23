using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

public class Message
{
    public int Id { get; set; }
    
    
    public int SenderId { get; set; }
    public AppUser Sender { get; set; }
    

    public int RecipientId { get; set; }
    public AppUser Recipient { get; set; }
    

    [Column(TypeName = "varchar(50)")]
    public MessageType MessageType { get; set; } = MessageType.Text;
    public string MessageContent { get; set; }

        
    public DateTime DateSent { get; set; } = DateTime.UtcNow;
    public bool Seen { get; set; }

    public Message()
    {
    }

    public Message(AppUser sender, AppUser recipient, MessageType messageType, string messageContent)
    {
        Sender = sender;
        Recipient = recipient;
        MessageType = messageType;
        MessageContent = messageContent;
    }
}