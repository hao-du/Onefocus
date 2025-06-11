using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.Banks)
                .WithOne(b => b.OwnerUser)
                .HasForeignKey(b => b.OwnerUserId);

            builder.HasMany(u => u.Currencies)
                .WithOne(c => c.OwnerUser)
                .HasForeignKey(c => c.OwnerUserId);

            builder.HasMany(u => u.Counterparties)
                .WithOne(c => c.OwnerUser)
                .HasForeignKey(c => c.OwnerUserId);

            builder.HasMany(u => u.Options)
                .WithOne(o => o.OwnerUser)
                .HasForeignKey(o => o.OwnerUserId);

            builder.HasMany(u => u.Transactions)
                .WithOne(t => t.OwnerUser)
                .HasForeignKey(t => t.OwnerUserId);

            builder.HasMany(builder => builder.BankAccounts)
                .WithOne(ba => ba.OwnerUser)
                .HasForeignKey(ba => ba.OwnerUserId);

            builder.HasQueryFilter(u => u.IsActive);
        }
    }
}