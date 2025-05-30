using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class CurrencyExchangeConfiguration : IEntityTypeConfiguration<CurrencyExchange>
    {
        public void Configure(EntityTypeBuilder<CurrencyExchange> builder)
        {
            builder.HasOne(ce => ce.BaseCurrency).WithMany(c => c.BaseCurrencyExchanges).HasForeignKey(ce => ce.BaseCurrencyId);
            builder.HasOne(ce => ce.TargetCurrency).WithMany(c => c.TargetCurrencyExchanges).HasForeignKey(ce => ce.TargetCurrencyId);
            builder.HasMany(ce => ce.Transactions).WithMany(t => t.CurrencyExchanges).UsingEntity(e => e.ToTable("CurrencyExchangeTransaction"));

            builder.HasQueryFilter(ce => ce.IsActive);
        }
    }
}