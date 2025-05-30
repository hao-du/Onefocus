using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class PeerTransferConfiguration : IEntityTypeConfiguration<PeerTransfer>
    {
        public void Configure(EntityTypeBuilder<PeerTransfer> builder)
        {
            builder.HasOne(pt => pt.TransferredUser).WithMany(u => u.PeerTransfers).HasForeignKey(pt => pt.TransferredUserId);
            builder.HasMany(pt => pt.Transactions).WithMany(t => t.PeerTransfers).UsingEntity(e => e.ToTable("PeerTransferTransaction"));

            builder.HasQueryFilter(pt => pt.IsActive);
        }
    }
}