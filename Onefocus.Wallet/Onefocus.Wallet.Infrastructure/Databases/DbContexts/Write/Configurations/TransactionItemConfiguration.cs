using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
    {
        public void Configure(EntityTypeBuilder<TransactionItem> builder)
        {
            builder.Property(ti => ti.Name).HasMaxLength(100);
            builder.Property(ti => ti.Description).HasMaxLength(255);

            builder.HasOne(ti => ti.Transaction).WithMany(t => t.TransactionItems).HasForeignKey(ti => ti.TransactionId);

            builder.HasQueryFilter(ti => ti.IsActive);
        }
    }
}
