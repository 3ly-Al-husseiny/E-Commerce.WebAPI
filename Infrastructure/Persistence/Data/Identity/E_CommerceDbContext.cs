using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Data.Identity
{
    public class E_CommerceIdentityDbContext(DbContextOptions<E_CommerceIdentityDbContext> options) : IdentityDbContext<AppUser>(options)

    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
            builder.Entity<Address>().ToTable("Addresses");
        }
    }
}
