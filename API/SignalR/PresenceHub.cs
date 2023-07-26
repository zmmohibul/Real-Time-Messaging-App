using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class PresenceHub : Hub
{
    private readonly PresenceTracker _presenceTracker;

    public PresenceHub(PresenceTracker presenceTracker)
    {
        _presenceTracker = presenceTracker;
    }
    
    public override async Task OnConnectedAsync()
    {
        var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
        var isOnline = _presenceTracker.UserConnected(username, Context.ConnectionId);

        if (isOnline)
        {
            await Clients.Others.SendAsync("UserIsOnline", username);
        }

        var currentUsers = _presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User.FindFirst(ClaimTypes.Name)?.Value;
        var isOffline =  _presenceTracker.UserDisconnected(username, Context.ConnectionId);
        
        if (isOffline)
        {
            await Clients.Others.SendAsync("UserIsOffline", username);
        }

        await base.OnDisconnectedAsync(exception);
    }
}