using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CurrencyExchangeConfiguration : IEntityTypeConfiguration<CurrencyExchange>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchange> builder)
        {
            builder.HasMany(ce => ce.CurrencyExchangeTransactions)
                .WithOne(cet => cet.CurrencyExchange)
                .HasForeignKey(ce => ce.CurrencyExchangeId);

            builder.HasQueryFilter(ce => ce.IsActive);
        }
    }
}