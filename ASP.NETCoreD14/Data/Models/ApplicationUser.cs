using Microsoft.AspNetCore.Identity;

namespace ASP.NETCoreD14.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
