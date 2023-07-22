using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.Dtos.Message;

public class MessageDto
{
    public int Id { get; set; }
    
    public string SenderUserName { get; set; }
    public string RecipientUserName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MessageType MessageType { get; set; }
    public string MessageContent { get; set; }
        
    public DateTime DateSent { get; set; }
    public bool Seen { get; set; }
}