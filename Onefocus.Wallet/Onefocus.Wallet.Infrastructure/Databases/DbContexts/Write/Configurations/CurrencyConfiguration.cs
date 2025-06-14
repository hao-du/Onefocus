using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class CurrencyConfiguration : BaseConfiguration<Currency>
    {
        public override void Configure(EntityTypeBuilder<Currency> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.ShortName).HasMaxLength(4).IsRequired();
            builder.Property(c => c.IsDefault).IsRequired();
            builder.Property(c => c.OwnerUserId).IsRequired();

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
