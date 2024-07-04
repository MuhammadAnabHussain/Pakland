using Microsoft.AspNetCore.Identity;

namespace Pakland.Models // Replace with your actual namespace
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties if needed

        public string FullName { get; set; }

    }
}
