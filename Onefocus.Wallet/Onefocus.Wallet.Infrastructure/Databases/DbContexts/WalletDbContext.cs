using Microsoft.EntityFrameworkCore;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Membership.Domain.Entities;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts;

public class WalletDbContext: DbContext
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

