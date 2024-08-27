using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

public class WalletWriteDbContext : DbContext
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
    public DbSet<TransactionDetail> TransactionDetail { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new CurrencyConfiguration());
        builder.ApplyConfiguration(new BankConfiguration());
        builder.ApplyConfiguration(new TransactionConfiguration());
        builder.ApplyConfiguration(new BankingTransactionConfiguration());
        builder.ApplyConfiguration(new ExchangeTransactionConfiguration());
        builder.ApplyConfiguration(new TransferTransactionConfiguration());
        builder.ApplyConfiguration(new TransactionDetailConfiguration());
    }
}

