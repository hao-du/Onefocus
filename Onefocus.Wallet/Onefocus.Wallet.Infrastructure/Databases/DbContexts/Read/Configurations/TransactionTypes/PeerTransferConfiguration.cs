using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class PeerTransferConfiguration : BaseConfiguration<PeerTransfer>
    {
        public override void Configure(EntityTypeBuilder<PeerTransfer> builder)
        {
            base.Configure(builder);
            builder.HasOne(ptt => ptt.Counterparty)
                   .WithMany(c => c.PeerTransfers)
                   .HasForeignKey(ptt => ptt.CounterpartyId);

            builder.HasMany(pt => pt.PeerTransferTransactions)
                .WithOne(ptt => ptt.PeerTransfer)
                .HasForeignKey(ptt => ptt.PeerTransferId);
        }
    }
}