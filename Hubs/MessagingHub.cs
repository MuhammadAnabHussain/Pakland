// MessagingHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Pakland.Hubs
{
    public class MessagingHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.User("admin").SendAsync("ReceiveMessage", user, message); // Send to admin
        }

        public async Task SendReply(string admin, string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", admin, message); // Send to specific user
        }
    }
}
