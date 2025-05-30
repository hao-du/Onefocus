﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Transactions).WithOne(tu => tu.User).HasForeignKey(tu => tu.UserId);
            builder.HasMany(u => u.PeerTransfers).WithOne(pt => pt.TransferredUser).HasForeignKey(pt => pt.TransferredUserId);

            builder.HasQueryFilter(u => u.IsActive);
        }
    }
}