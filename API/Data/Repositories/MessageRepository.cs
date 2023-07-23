using API.Dtos.Message;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<Message> GetMessageById(int id)
    {
        return await _dataContext.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<MessageDto> GetMessageDtoById(int id)
    {
        return await _dataContext.Messages
            .AsNoTracking()
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> CreateMessage(Message message)
    {
        _dataContext.Messages.Add(message);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUserName, string otherUserName)
    {
        var messages = await _dataContext.Messages
            .Where(u =>
                (u.Sender.UserName.Equals(currentUserName) && u.Recipient.UserName.Equals(otherUserName))
                || (u.Recipient.UserName.Equals(currentUserName) && u.Sender.UserName.Equals(otherUserName))
            )
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .OrderBy(m => m.DateSent)
            .ToListAsync();


        var i = messages.Count - 1;
        if (i >= 0 && messages[i].Recipient.UserName.Equals(currentUserName))
        {
            messages[i].Seen = true;
        }
        await _dataContext.SaveChangesAsync();

        return _mapper.Map<IEnumerable<MessageDto>>(messages);
    }

    public async Task<bool> DeleteMessageAsync(Message message)
    {
        _dataContext.Messages.Remove(message);
        return await _dataContext.SaveChangesAsync() > 0;
    }
}