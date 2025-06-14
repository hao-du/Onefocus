using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class CurrencyConfiguration : BaseConfiguration<Currency>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            base.Configure(builder);

            builder.HasMany(c => c.Transactions)
                .WithOne(t => t.Currency)
                .HasForeignKey(t => t.CurrencyId);

            builder.HasMany(c => c.BankAccounts)
                .WithOne(ba => ba.Currency)
                .HasForeignKey(ba => ba.CurrencyId);

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Currencies)
                .HasForeignKey(c => c.OwnerUserId);
        }
    }
}
