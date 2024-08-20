using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts;

public class WalletWriteDbContext: DbContext
{
    public WalletWriteDbContext(DbContextOptions<WalletWriteDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

