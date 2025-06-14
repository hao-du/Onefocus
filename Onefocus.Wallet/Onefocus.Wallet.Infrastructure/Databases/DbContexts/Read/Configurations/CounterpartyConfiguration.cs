﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class CounterpartyConfiguration : BaseConfiguration<Counterparty>
    {
        public override void Configure(EntityTypeBuilder<Counterparty> builder)
        {
            base.Configure(builder);
            builder.HasMany(c => c.PeerTransfers)
                .WithOne(pt => pt.Counterparty)
                .HasForeignKey(pt => pt.CounterpartyId);

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Counterparties)
                .HasForeignKey(c => c.OwnerUserId);
        }
    }
}
