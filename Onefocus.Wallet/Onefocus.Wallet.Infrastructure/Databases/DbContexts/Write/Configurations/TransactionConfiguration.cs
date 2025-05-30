using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.Description).HasMaxLength(255);

            builder.HasOne(t => t.User).WithMany(u => u.Transactions).HasForeignKey(t => t.Id);
            builder.HasOne(t => t.Currency).WithMany(c => c.Transactions).HasForeignKey(t => t.Id);

            builder.HasMany(t => t.TransactionItems).WithOne(ti => ti.Transaction).HasForeignKey(ti => ti.TransactionId);
            builder.HasMany(t => t.PeerTransfers).WithMany(pt => pt.Transactions).UsingEntity(e => e.ToTable("PeerTransferTransaction"));
            builder.HasMany(t => t.BankAccounts).WithMany(ba => ba.Transactions).UsingEntity(e => e.ToTable("BankAccountTransaction"));
            builder.HasMany(t => t.CashFlows).WithMany(cf => cf.Transactions).UsingEntity(e => e.ToTable("CastFlowTransaction"));
            builder.HasMany(t => t.CurrencyExchanges).WithMany(ce => ce.Transactions).UsingEntity(e => e.ToTable("CurrencyExchangeTransaction"));

            builder.HasQueryFilter(t => t.IsActive);
        }
    }
}
