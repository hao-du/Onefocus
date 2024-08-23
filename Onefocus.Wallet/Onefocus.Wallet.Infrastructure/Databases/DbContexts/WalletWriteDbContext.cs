using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts;

public class WalletWriteDbContext: DbContext
{
    public WalletWriteDbContext(DbContextOptions<WalletWriteDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;
    public DbSet<Bank> Bank { get; set; } = default!;
    public DbSet<Currency> Currency { get; set; } = default!;
    public DbSet<IncomeTransaction> IncomeTransaction { get; set; }
    public DbSet<OutcomeTransaction> OutcomeTransaction { get; set; }
    public DbSet<TransferTransaction> TransferTransaction { get; set; }
    public DbSet<BankingTransaction> BankingTransaction { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Transaction>().UseTpcMappingStrategy();
    }
}

