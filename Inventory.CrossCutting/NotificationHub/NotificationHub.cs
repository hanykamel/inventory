using Microsoft.AspNetCore.SignalR;

namespace Inventory.CrossCutting.NotificationHub
{
    public class NotificationHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}