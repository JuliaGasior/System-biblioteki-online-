using Microsoft.AspNetCore.Identity;

namespace SystemBibliotekiOnline.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}