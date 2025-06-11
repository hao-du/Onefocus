using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class TransactionItemConfiguration : IEntityTypeConfiguration<TransactionItem>
    {
        public void Configure(EntityTypeBuilder<TransactionItem> builder)
        {
            builder.HasOne(ti => ti.Transaction)
                .WithMany(t => t.TransactionItems)
                .HasForeignKey(ti => ti.TransactionId);

            builder.HasQueryFilter(ti => ti.IsActive);
        }
    }
}
