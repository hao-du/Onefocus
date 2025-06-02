using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.Property(b => b.Name).HasMaxLength(100);
            builder.Property(b => b.Description).HasMaxLength(255);

            builder.HasMany(b => b.BankAccounts).WithOne(bt => bt.Bank).HasForeignKey(bt => bt.BankId);

            builder.HasQueryFilter(b => b.IsActive);
        }
    }
}
