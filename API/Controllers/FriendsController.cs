using API.Extensions;
using API.Interfaces;
using API.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FriendsController : ControllerBase
{
    private readonly IFriendRepository _friendRepository;
    private readonly IHubContext<EventHub> _eventHub;

    public FriendsController(
        IFriendRepository friendRepository,
        IHubContext<EventHub> eventHub
        )
    {
        _friendRepository = friendRepository;
        _eventHub = eventHub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFriends()
    {
        return Ok(await _friendRepository.GetAllFriendsForUser(User.GetUserId()));
    }
    
    [HttpPost("{friendId}")]
    public async Task<IActionResult> RemoveFriend(int friendId)
    {
        return Ok();
    }
}