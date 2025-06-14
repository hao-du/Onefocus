using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class BankConfiguration : BaseConfiguration<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            base.Configure(builder);
            builder.HasMany(b => b.BankAccounts)
                .WithOne(ba => ba.Bank)
                .HasForeignKey(ba => ba.BankId);

            builder.HasOne(b => b.OwnerUser)
                .WithMany(u => u.Banks)
                .HasForeignKey(b => b.OwnerUserId);
        }
    }
}
