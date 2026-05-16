using Microsoft.AspNetCore.Identity;

namespace ASP.NETCoreD14.Data.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
