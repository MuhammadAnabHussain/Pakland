using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Pakland.Data;
using Pakland.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pakland.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/Notification/GetUnreadNotifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _context.BuyingNotifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpPut]
        [Route("/Notification/MarkAsRead/{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            // Find the notification in the database and mark it as read
            var notification = await _context.BuyingNotifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound();
            }

            notification.IsSeen = true;
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
