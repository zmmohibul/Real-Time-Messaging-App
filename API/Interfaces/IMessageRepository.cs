using API.Dtos.Message;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMessageRepository
{
    Task<Message> GetMessageById(int id);
    Task<MessageDto> GetMessageDtoById(int id);
    Task<bool> CreateMessage(Message message);
    Task<bool> DeleteMessageAsync(Message message);
    Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUserName, string otherUserName);
}