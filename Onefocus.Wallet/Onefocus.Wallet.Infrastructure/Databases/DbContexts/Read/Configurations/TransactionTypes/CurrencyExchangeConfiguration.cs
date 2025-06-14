using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CurrencyExchangeConfiguration : BaseConfiguration<CurrencyExchange>
    {
        public override void Configure(EntityTypeBuilder<CurrencyExchange> builder)
        {
            base.Configure(builder);
            builder.HasMany(ce => ce.CurrencyExchangeTransactions)
                .WithOne(cet => cet.CurrencyExchange)
                .HasForeignKey(ce => ce.CurrencyExchangeId);
        }
    }
}