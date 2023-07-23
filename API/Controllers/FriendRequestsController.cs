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
    private readonly IMapper _mapper;
    private readonly IHubContext<PresenceHub> _eventHub;

    public FriendRequestsController(IFriendRequestRepository friendRequestRepository, IUserRepository userRepository, IMapper mapper, IHubContext<PresenceHub> eventHub)
    {
        _friendRequestRepository = friendRequestRepository;
        _userRepository = userRepository;
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
                await _eventHub.Clients.Clients(connections).SendAsync("NewFriendRequest", _mapper.Map<UserDetailsDto>(requestFrom));
            }
            
            return Ok("Request sent");
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
}