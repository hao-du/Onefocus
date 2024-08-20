using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts;

public class WalletReadDbContext : DbContext
{
    public WalletReadDbContext(DbContextOptions<WalletReadDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

