using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

public class WalletWriteDbContext(DbContextOptions<WalletWriteDbContext> options) : DbContext(options)
{
    public DbSet<Bank> Bank { get; set; }
    public DbSet<Counterparty> Counterparty { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Option> Option { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<TransactionItem> TransactionItem { get; set; }
    public DbSet<User> User { get; set; }

    public DbSet<BankAccount> BankAccount { get; set; }
    public DbSet<BankAccountTransaction> BankAccountTransaction { get; set; }
    public DbSet<CashFlow> CashFlow { get; set; }
    public DbSet<CurrencyExchange> CurrencyExchange { get; set; }
    public DbSet<CurrencyExchangeTransaction> CurrencyExchangeTransaction { get; set; }
    public DbSet<PeerTransfer> PeerTransfer { get; set; }
    public DbSet<PeerTransferTransaction> PeerTransferTransaction { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BankAccountConfiguration());
        builder.ApplyConfiguration(new CounterpartyConfiguration());


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

