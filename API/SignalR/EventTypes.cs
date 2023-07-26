using System.Runtime.Serialization;

namespace API.SignalR;

public enum EventTypes
{
    [EnumMember(Value = "UserIsOnline")]
    UserIsOnline,
    
    [EnumMember(Value = "UserIsOffline")]
    UserIsOffline,
    
    [EnumMember(Value = "GetOnlineUsers")]
    GetOnlineUsers,
    
    [EnumMember(Value = "NewFriendRequest")]
    NewFriendRequest,
    
    [EnumMember(Value = "FriendRequestAccepted")]
    FriendRequestAccepted
}