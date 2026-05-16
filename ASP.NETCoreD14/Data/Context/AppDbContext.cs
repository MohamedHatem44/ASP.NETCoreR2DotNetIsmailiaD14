using ASP.NETCoreD14.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD14.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /*------------------------------------------------------------------*/
        public AppDbContext() : base()
        {
        }
        /*------------------------------------------------------------------*/
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        /*------------------------------------------------------------------*/
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        /*------------------------------------------------------------------*/
    }
}
