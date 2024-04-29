using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Entities.Identity
{
    public class ApplicationUsers : IdentityUser
    {
        public string DisplayName { get; set; } = null!;

        public Address? Address { get; set; } = null; // Navigational Property [One]

    }
}
