//NotificationHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Pakland.Models;
using Pakland.Services;
using Pakland.Data;

namespace Pakland.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
    }

}
