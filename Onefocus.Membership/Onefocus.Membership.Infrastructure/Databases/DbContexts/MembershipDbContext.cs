using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Membership.Infrastructure.Databases.DbContexts;

internal class MembershipDbContext(DbContextOptions<MembershipDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(user =>
        {
            user.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            user.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        });
    }
}

