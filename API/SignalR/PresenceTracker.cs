namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, HashSet<string>> OnlineUsers = new Dictionary<string, HashSet<string>>();

    public bool UserConnected(string username, string connectionId)
    {
        bool isOnline = false;
        lock(OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId);
            }
            else 
            {
                OnlineUsers.Add(username, new HashSet<string>{connectionId});
                isOnline = true;
            }
        }

        return isOnline;
    }

    public bool UserDisconnected(string username, string connectionId)
    {
        bool isOffline = false;

        lock(OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(username))
            {
                return isOffline;
            }

            OnlineUsers[username].Remove(connectionId);

            if (OnlineUsers[username].Count == 0)
            {
                OnlineUsers.Remove(username);
                isOffline = true;
            }
        }

        return isOffline;
    }

    public string[] GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.Keys.ToArray();
        }

        return onlineUsers;
    }
}