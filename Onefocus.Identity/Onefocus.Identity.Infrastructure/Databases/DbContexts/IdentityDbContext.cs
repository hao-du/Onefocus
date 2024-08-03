using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Onefocus.Identity.Domain.Entities;

namespace Onefocus.Identity.Infrastructure.Databases.DbContexts;

internal class IdentityDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }
}