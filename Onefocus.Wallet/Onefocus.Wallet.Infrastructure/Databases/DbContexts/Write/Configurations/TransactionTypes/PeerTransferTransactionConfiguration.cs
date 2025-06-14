using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class PeerTransferTransactionConfiguration : BaseConfiguration<PeerTransferTransaction>
    {
        public override void Configure(EntityTypeBuilder<PeerTransferTransaction> builder)
        {
            base.Configure(builder);
            builder.Property(f => f.PeerTransferId).IsRequired();
            builder.Property(f => f.TransactionId).IsRequired();
            builder.Property(f => f.IsInFlow).IsRequired();

            builder.HasOne(ptt => ptt.Transaction)
                   .WithMany(t => t.PeerTransferTransactions)
                   .HasForeignKey(ptt => ptt.TransactionId);

            builder.HasOne(ptt => ptt.PeerTransfer)
                   .WithMany(pt => pt.PeerTransferTransactions)
                   .HasForeignKey(ptt => ptt.PeerTransferId);

            builder.HasQueryFilter(pt => pt.IsActive);
        }
    }
}