using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pakland.Models;

namespace Pakland.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PropertyDetails>? PropertyDetails { get; set; }
        public DbSet<BuyingNotifications> BuyingNotifications { get; set; }

    }
}
