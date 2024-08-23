﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            builder.Property(p => p.Description).HasMaxLength(255);

            builder.HasOne(td => td.Transaction).WithMany(t => t.TransactionDetails).HasForeignKey(td => td.Id);
        }
    }
}