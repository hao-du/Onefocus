using Microsoft.EntityFrameworkCore;
using Onefocus.Wallet.Domain.Entities.Write;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

public class WalletWriteDbContext(DbContextOptions<WalletWriteDbContext> options) : DbContext(options)
{
    public required DbSet<Bank> Bank { get; set; }
    public required DbSet<Counterparty> Counterparty { get; set; }
    public required DbSet<Currency> Currency { get; set; }
    public required DbSet<SearchIndexQueue> SearchIndexQueue { get; set; }
    public required DbSet<Transaction> Transaction { get; set; }
    public required DbSet<TransactionItem> TransactionItem { get; set; }
    public required DbSet<User> User { get; set; }
    public required DbSet<BankAccount> BankAccount { get; set; }
    public required DbSet<BankAccountTransaction> BankAccountTransaction { get; set; }
    public required DbSet<CashFlow> CashFlow { get; set; }
    public required DbSet<CurrencyExchange> CurrencyExchange { get; set; }
    public required DbSet<CurrencyExchangeTransaction> CurrencyExchangeTransaction { get; set; }
    public required DbSet<PeerTransfer> PeerTransfer { get; set; }
    public required DbSet<PeerTransferTransaction> PeerTransferTransaction { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new BankAccountConfiguration());
        builder.ApplyConfiguration(new CounterpartyConfiguration());
        builder.ApplyConfiguration(new SearchIndexQueueConfiguration());
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

