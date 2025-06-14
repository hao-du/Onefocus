using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class CurrencyExchangeConfiguration : BaseConfiguration<CurrencyExchange>
    {
        public override void Configure(EntityTypeBuilder<CurrencyExchange> builder)
        {
            builder.Property(f => f.ExchangeRate).HasPrecision(18, 2).IsRequired();

            builder.HasMany(ce => ce.CurrencyExchangeTransactions)
                .WithOne(cet => cet.CurrencyExchange)
                .HasForeignKey(ce => ce.CurrencyExchangeId);
        }
    }
}