using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

public class WalletWriteDbContext(DbContextOptions<WalletWriteDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = default!;
    public DbSet<Bank> Bank { get; set; } = default!;
    public DbSet<Currency> Currency { get; set; } = default!;
    public DbSet<BankAccount> BankAccount { get; set; }
    public DbSet<CashFlow> CashFlow { get; set; }
    public DbSet<CurrencyExchange> CurrencyExchange { get; set; }
    public DbSet<PeerTransfer> PeerTransfer { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<TransactionItem> TransactionItem { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BankAccountConfiguration());
        builder.ApplyConfiguration(new CashFlowConfiguration());
        builder.ApplyConfiguration(new CurrencyExchangeConfiguration());
        builder.ApplyConfiguration(new PeerTransferConfiguration());
        builder.ApplyConfiguration(new BankConfiguration());
        builder.ApplyConfiguration(new CurrencyConfiguration());
        builder.ApplyConfiguration(new TransactionConfiguration());
        builder.ApplyConfiguration(new TransactionItemConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
    }
}

