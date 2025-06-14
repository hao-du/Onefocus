using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class CounterpartyConfiguration : BaseConfiguration<Counterparty>
    {
        public override void Configure(EntityTypeBuilder<Counterparty> builder)
        {
            base.Configure(builder);
            builder.Property(f => f.FullName).HasMaxLength(100).IsRequired();
            builder.Property(f => f.Email).HasMaxLength(254);
            builder.Property(f => f.PhoneNumber).HasMaxLength(25);
            builder.Property(f => f.OwnerUserId).IsRequired();

            builder.HasMany(c => c.PeerTransfers)
                .WithOne(pt => pt.Counterparty)
                .HasForeignKey(pt => pt.CounterpartyId);

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Counterparties)
                .HasForeignKey(c => c.OwnerUserId);
        }
    }
}
