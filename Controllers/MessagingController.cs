using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;

using Pakland.Services;
using Pakland.Data;
using Pakland.Models;
using Pakland.Hubs;

namespace Pakland.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MessagingController : Controller
    {
        private readonly IHubContext<MessagingHub> _hubContext;

        public MessagingController(IHubContext<MessagingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> ReplyToMessage(string user, string message)
        {
            // Send message to the specific user using SignalR hub
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", user, message);

            return Ok();
        }
    }
}
