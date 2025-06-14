using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class BankConfiguration : BaseConfiguration<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            base.Configure(builder);
            builder.Property(f => f.Name).HasMaxLength(100).IsRequired();
            builder.Property(f => f.OwnerUserId).IsRequired();

            builder.HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.Bank)
                .HasForeignKey(ba => ba.BankId);

            builder.HasOne(b => b.OwnerUser)
                .WithMany(u => u.Banks)
                .HasForeignKey(b => b.OwnerUserId);
        }
    }
}
