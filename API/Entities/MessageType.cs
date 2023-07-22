using System.Runtime.Serialization;

namespace API.Entities;

public enum MessageType
{
    [EnumMember(Value = "Text")]
    Text,
    
    [EnumMember(Value = "PictureUrl")]
    PictureUrl,
}