using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class CurrencyExchangeTransactionConfiguration : IEntityTypeConfiguration<CurrencyExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchangeTransaction> builder)
        {
            builder.Property(f => f.CurrencyExchangeId).IsRequired();
            builder.Property(f => f.TransactionId).IsRequired();
            builder.Property(f => f.IsTarget).IsRequired();

            builder.HasOne(cet => cet.Transaction)
                .WithMany(t => t.CurrencyExchangeTransactions)
                .HasForeignKey(cet => cet.TransactionId);

            builder.HasOne(cet => cet.CurrencyExchange)
                .WithMany(ce => ce.CurrencyExchangeTransactions)
                .HasForeignKey(cet => cet.CurrencyExchangeId);
        }
    }
}