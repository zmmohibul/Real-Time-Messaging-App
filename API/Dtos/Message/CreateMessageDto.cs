using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.Dtos.Message;

public class CreateMessageDto
{
    [Required]
    public string RecipientUserName { get; set; }

    [Required] [JsonConverter(typeof(JsonStringEnumConverter))]
    public MessageType MessageType { get; set; }
    
    [Required]
    public string MessageContent { get; set; }
}