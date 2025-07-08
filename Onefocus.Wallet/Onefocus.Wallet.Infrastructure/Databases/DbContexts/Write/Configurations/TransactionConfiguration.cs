using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionConfiguration : BaseConfiguration<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(t => t.TransactedOn).IsRequired();
            builder.Property(t => t.CurrencyId).IsRequired();
            builder.Property(t => t.OwnerUserId).IsRequired();


            builder.HasMany(t => t.BankAccountTransactions).WithOne(bat => bat.Transaction).HasForeignKey(bat => bat.TransactionId);
            builder.HasMany(t => t.PeerTransferTransactions).WithOne(pt => pt.Transaction).HasForeignKey(pt => pt.TransactionId);
            builder.HasMany(t => t.CurrencyExchangeTransactions).WithOne(ce => ce.Transaction).HasForeignKey(ce => ce.TransactionId);
            builder.HasMany(t => t.CashFlows).WithOne(cf => cf.Transaction).HasForeignKey(cf => cf.TransactionId);
            builder.HasMany(t => t.TransactionItems).WithOne(ti => ti.Transaction).HasForeignKey(ti => ti.TransactionId);
            builder.HasOne(t => t.OwnerUser).WithMany(u => u.Transactions).HasForeignKey(t => t.OwnerUserId);
            builder.HasOne(t => t.Currency).WithMany(c => c.Transactions).HasForeignKey(t => t.CurrencyId);
        }
    }
}
