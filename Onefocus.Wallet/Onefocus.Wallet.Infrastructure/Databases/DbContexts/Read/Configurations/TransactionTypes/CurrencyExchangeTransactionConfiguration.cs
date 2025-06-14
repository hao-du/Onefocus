using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CurrencyExchangeTransactionConfiguration : BaseConfiguration<CurrencyExchangeTransaction>
    {
        public override void Configure(EntityTypeBuilder<CurrencyExchangeTransaction> builder)
        {
            base.Configure(builder);

            builder.HasOne(cet => cet.Transaction)
                .WithMany(t => t.CurrencyExchangeTransactions)
                .HasForeignKey(cet => cet.TransactionId);

            builder.HasOne(cet => cet.CurrencyExchange)
                .WithMany(ce => ce.CurrencyExchangeTransactions)
                .HasForeignKey(cet => cet.CurrencyExchangeId);
        }
    }
}