using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class TransactionItemConfiguration : BaseConfiguration<TransactionItem>
    {
        public override void Configure(EntityTypeBuilder<TransactionItem> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
            builder.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(t => t.TransactionId).IsRequired();

            builder.HasOne(ti => ti.Transaction)
                .WithMany(t => t.TransactionItems)
                .HasForeignKey(ti => ti.TransactionId);
        }
    }
}
