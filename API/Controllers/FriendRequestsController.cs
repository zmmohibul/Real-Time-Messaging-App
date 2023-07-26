using System.Security.Claims;
using API.Data;
using API.Dtos.FriendRequest;
using API.Dtos.User;
using API.Errors;
using API.Extensions;
using API.Interfaces;
using API.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FriendRequestsController : ControllerBase
{
    private readonly IFriendRequestRepository _friendRequestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFriendRepository _friendRepository;
    private readonly IMapper _mapper;
    private readonly IHubContext<EventHub> _eventHub;

    public FriendRequestsController(
        IFriendRequestRepository friendRequestRepository,
        IUserRepository userRepository,
        IFriendRepository friendRepository,
        IMapper mapper,
        IHubContext<EventHub> eventHub
        )
    {
        _friendRequestRepository = friendRequestRepository;
        _userRepository = userRepository;
        _friendRepository = friendRepository;
        _mapper = mapper;
        _eventHub = eventHub;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRequest()
    {
        return Ok(await _friendRequestRepository.GetAllRequestsForUser(User.GetUserId()));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFriendRequest([FromQuery] int requestToUserId)
    {
        var requestFrom = await _userRepository.GetUserByUserIdAsync(User.GetUserId());

        var requestTo = await _userRepository.GetUserByUserIdAsync(requestToUserId);
        if (requestTo == null)
        {
            return NotFound(new Error(404, "Recipient user not found"));
        }

        if (requestFrom.Id == requestToUserId)
        {
            return BadRequest(new Error(400, "You cannot send request to yourself"));
        }

        if (await _friendRepository.AreFriends(requestFrom.Id, requestTo.Id))
        {
            return BadRequest(new Error(400, "You are already friends with this user"));
        }

        if (await _friendRequestRepository.RequestExist(requestFrom.Id, requestTo.Id))
        {
            return BadRequest(new Error(400, "You have already sent a request to this user"));
        }
        
        if (await _friendRequestRepository.RequestExist(requestTo.Id, requestFrom.Id))
        {
            return BadRequest(new Error(400, "This user has already sent you a friend request. Please accept to be friends"));
        }

        if (await _friendRequestRepository.CreateRequest(requestFrom, requestTo))
        {
            var connections = PresenceTracker.GetConnectionsForUser(requestTo.UserName);
            if (connections != null)
            {
                await _eventHub.Clients.Clients(connections).SendAsync($"{EventTypes.NewFriendRequest}", _mapper.Map<UserDetailsDto>(requestFrom));
            }
            
            return Ok(new { StatusCode = 200, Message = "Request sent" });
        }

        return BadRequest(new Error(400, "Could not send friend request"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFriendRequest(int id)
    {
        var request = await _friendRequestRepository.GetRequestsById(id);

        if (request == null)
        {
            return NotFound(new Error(404, "Invalid request id"));
        }

        if (request.RequestFromId != User.GetUserId())
        {
            return Unauthorized(new Error(401, "You are not the sender of this request"));
        }

        if (await _friendRequestRepository.DeleteFriendRequestById(id))
        {
            return NoContent();
        }

        return BadRequest(new Error(400, "Could not delete friend request"));
    }
    
    [HttpPost("accept/{requestId}")]
    public async Task<IActionResult> AcceptFriendRequest(int requestId)
    {
        var friendRequest = await _friendRequestRepository.GetRequestsById(requestId);
        if (friendRequest == null)
        {
            return NotFound(new Error(404, "Invalid friend request"));
        }
        
        if (friendRequest.RequestToId != User.GetUserId())
        {
            return Unauthorized(new Error(401, "This request was not for you"));
        }

        var currentUser = await _userRepository.GetUserByUserIdAsync(User.GetUserId());
        var friendUser = await _userRepository.GetUserByUserIdAsync(friendRequest.RequestFromId);
        
        var newfriend = await _friendRepository.AddNewFriend(currentUser, friendUser);
        if (newfriend == null)
        {
            return BadRequest(new Error(400, "Could not add friend"));
        }

        await _friendRequestRepository.DeleteFriendRequestById(friendRequest.Id);
        
        var connections = PresenceTracker.GetConnectionsForUser(friendRequest.RequestFrom.UserName);
        if (connections != null)
        {
            await _eventHub.Clients.Clients(connections).SendAsync($"{EventTypes.FriendRequestAccepted}", _mapper.Map<UserDetailsDto>(friendRequest.RequestFrom));
        }
        return Ok(newfriend);
    }
}