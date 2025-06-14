using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class TransactionItemConfiguration : BaseConfiguration<TransactionItem>
    {
        public override void Configure(EntityTypeBuilder<TransactionItem> builder)
        {
            base.Configure(builder);
            builder.HasOne(ti => ti.Transaction)
                .WithMany(t => t.TransactionItems)
                .HasForeignKey(ti => ti.TransactionId);
        }
    }
}
