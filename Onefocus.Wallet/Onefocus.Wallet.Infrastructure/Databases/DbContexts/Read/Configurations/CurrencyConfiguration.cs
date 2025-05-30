using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.ShortName).HasMaxLength(4);
            builder.Property(p => p.Description).HasMaxLength(255);

            builder.HasMany(c => c.ExchangeTransactions).WithOne(et => et.ExchangedCurrency).HasForeignKey(et => et.ExchangedCurrencyId);
            builder.HasMany(c => c.Transactions).WithOne(et => et.Currency).HasForeignKey(et => et.CurrencyId);

            builder.HasQueryFilter(c => c.IsActive);
        }
    }
}
