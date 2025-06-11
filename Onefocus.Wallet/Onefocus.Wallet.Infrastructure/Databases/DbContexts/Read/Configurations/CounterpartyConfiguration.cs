using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class CounterpartyConfiguration : IEntityTypeConfiguration<Counterparty>
    {
        public void Configure(EntityTypeBuilder<Counterparty> builder)
        {
            builder.HasMany(c => c.PeerTransferTransactions)
                .WithOne(pt => pt.Counterparty)
                .HasForeignKey(pt => pt.CounterpartyId);

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Counterparties)
                .HasForeignKey(c => c.OwnerUserId);

            builder.HasQueryFilter(b => b.IsActive);
        }
    }
}
