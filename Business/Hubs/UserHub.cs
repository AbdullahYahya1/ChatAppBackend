using Business.Entities;
using Business.IServices;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class UserHub : Hub
    {

        private static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string userEmail = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userEmail))
            {
                ConnectedUsers[userEmail] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string userEmail = Context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userEmail))
            {
                ConnectedUsers.TryRemove(userEmail, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }
        public static bool IsUserOnline(string email)
        {
            return ConnectedUsers.ContainsKey(email);
        }
        public async Task SendFriendRequestNotification(string receiverUserId, string senderUserName)
        {
            await Clients.User(receiverUserId).SendAsync("ReceiveFriendRequest", senderUserName);
        }

    }
}