﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class PeerTransferTransactionConfiguration : BaseConfiguration<PeerTransferTransaction>
    {
        public override void Configure(EntityTypeBuilder<PeerTransferTransaction> builder)
        {
            base.Configure(builder);
            builder.HasOne(ptt => ptt.Transaction)
                   .WithMany(t => t.PeerTransferTransactions)
                   .HasForeignKey(ptt => ptt.TransactionId);

            builder.HasOne(ptt => ptt.PeerTransfer)
                   .WithMany(pt => pt.PeerTransferTransactions)
                   .HasForeignKey(ptt => ptt.PeerTransferId);
        }
    }
}