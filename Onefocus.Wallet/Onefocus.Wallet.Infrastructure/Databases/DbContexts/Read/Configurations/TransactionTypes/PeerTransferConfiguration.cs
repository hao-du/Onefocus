using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class PeerTransferConfiguration : IEntityTypeConfiguration<PeerTransfer>
    {
        public void Configure(EntityTypeBuilder<PeerTransfer> builder)
        {
            builder.HasMany(pt => pt.PeerTransferTransactions)
                .WithOne(ptt => ptt.PeerTransfer)
                .HasForeignKey(ptt => ptt.PeerTransferId);

            builder.HasQueryFilter(pt => pt.IsActive);
        }
    }
}