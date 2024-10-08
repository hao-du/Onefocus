﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.UseTptMappingStrategy();

            builder.Property(p => p.Description).HasMaxLength(255);

            builder.HasOne(t => t.User).WithMany(u => u.Transactions).HasForeignKey(t => t.Id);
            builder.HasOne(t => t.Currency).WithMany(c => c.Transactions).HasForeignKey(t => t.Id);
            builder.HasMany(t => t.TransactionDetails).WithOne(td => td.Transaction).HasForeignKey(td => td.Id);
        }
    }
}
