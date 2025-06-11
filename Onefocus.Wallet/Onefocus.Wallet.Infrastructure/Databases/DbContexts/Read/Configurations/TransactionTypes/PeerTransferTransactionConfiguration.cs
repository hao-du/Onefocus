using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class PeerTransferTransactionConfiguration : IEntityTypeConfiguration<PeerTransferTransaction>
    {
        public void Configure(EntityTypeBuilder<PeerTransferTransaction> builder)
        {
            builder.HasOne(ptt => ptt.Counterparty)
                   .WithMany(c => c.PeerTransferTransactions)
                   .HasForeignKey(ptt => ptt.CounterpartyId);

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