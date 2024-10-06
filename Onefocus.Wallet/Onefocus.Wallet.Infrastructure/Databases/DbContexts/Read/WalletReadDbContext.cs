using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

public class WalletReadDbContext : DbContext
{
    public WalletReadDbContext(DbContextOptions<WalletReadDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;
    public DbSet<Bank> Bank { get; set; } = default!;
    public DbSet<Currency> Currency { get; set; } = default!;
    public DbSet<IncomeTransaction> IncomeTransaction { get; set; }
    public DbSet<OutcomeTransaction> OutcomeTransaction { get; set; }
    public DbSet<TransferTransaction> TransferTransaction { get; set; }
    public DbSet<BankingTransaction> BankingTransaction { get; set; }
    public DbSet<ExchangeTransaction> ExchangeTransaction { get; set; }
    public DbSet<TransactionDetail> TransactionDetail { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(WalletReadDbContext).Assembly);
    }
}
