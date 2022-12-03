using Microsoft.AspNetCore.Identity;

namespace CarDealership.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
