using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();

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
        }
    }
}