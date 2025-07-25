﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class OptionConfiguration : BaseConfiguration<Option>
    {
        public override void Configure(EntityTypeBuilder<Option> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Options)
                .HasForeignKey(c => c.OwnerUserId);
        }
    }
}
