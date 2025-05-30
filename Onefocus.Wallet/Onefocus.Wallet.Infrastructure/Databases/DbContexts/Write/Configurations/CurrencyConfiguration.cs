using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.Property(c => c.Name).HasMaxLength(100);
            builder.Property(c => c.ShortName).HasMaxLength(4);
            builder.Property(c => c.Description).HasMaxLength(255);

            builder.HasMany(c => c.Transactions).WithOne(t => t.Currency).HasForeignKey(t => t.CurrencyId);
            builder.HasMany(c => c.BankAccounts).WithOne(ba => ba.Currency).HasForeignKey(ba => ba.CurrencyId);
            builder.HasMany(c => c.BaseCurrencyExchanges).WithOne(ce => ce.BaseCurrency).HasForeignKey(ce => ce.BaseCurrencyId);
            builder.HasMany(c => c.TargetCurrencyExchanges).WithOne(ce => ce.TargetCurrency).HasForeignKey(ce => ce.TargetCurrencyId);

            builder.HasQueryFilter(c => c.IsActive);
        }
    }
}
