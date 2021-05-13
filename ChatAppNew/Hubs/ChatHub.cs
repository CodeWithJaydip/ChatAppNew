using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppNew.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppNew.Hubs
{
    public class ChatHub:Hub
    {
        public string getConnectionId()
        {
            return Context.ConnectionId;
        }

        public override  Task OnConnectedAsync()
        {
            ConnectedUser.Ids.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUser.Ids.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
