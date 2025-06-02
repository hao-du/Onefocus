using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.DbContexts;

internal class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(p => p.MembershipUserId).IsRequired();
        builder.Entity<User>().HasIndex(p => p.MembershipUserId).IsUnique();
    }
}