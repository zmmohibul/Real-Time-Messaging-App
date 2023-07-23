using System.Security.Claims;
using API.Data;
using API.Dtos.Message;
using API.Entities;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesController(DataContext dataContext, IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
    {
        _dataContext = dataContext;
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpGet("thread/{userName}")]
    public async Task<IActionResult> GetMessageThread(string userName)
    {
        return Ok(await _messageRepository.GetMessageThreadAsync(User.FindFirst(ClaimTypes.Name)?.Value,
            userName));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
    {
        var sender = await _userRepository.GetUserByUserNameAsync(User.FindFirst(ClaimTypes.Name)?.Value);
        
        var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);
        if (recipient == null)
        {
            return NotFound(new Error(404,"Recipient user not found"));
        }

        var message = new Message(sender, recipient, createMessageDto.MessageType, createMessageDto.MessageContent);

        if (await _messageRepository.CreateMessage(message))
        {
            return Ok(_mapper.Map<MessageDto>(message));
        }

        return BadRequest(new Error(400, "Failed to send message"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var message = await _messageRepository.GetMessageById(id);
        if (message == null)
        {
            return NotFound(new Error(404, "Already Deleted Message"));
        }

        var currentUser =
            await _userRepository.GetUserByUserNameAsync(User.FindFirst(ClaimTypes.Name)?.Value);
        if (!message.Sender.UserName.Equals(currentUser.UserName))
        {
            return BadRequest(new Error(401, "You are not the sender of this message"));
        }

        if (await _messageRepository.DeleteMessageAsync(message))
        {
            return NoContent();
        }

        return BadRequest(new Error(400, "Could not delete message"));
    }
}