using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetworkWeb.Models;

namespace SocialNetworkWeb.Data
{
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}