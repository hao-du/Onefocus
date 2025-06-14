using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class CashFlowConfiguration : BaseConfiguration<CashFlow>
    {
        public override void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.TransactionId).IsRequired();
            builder.Property(f => f.IsIncome).IsRequired();

            builder.HasOne(bat => bat.Transaction)
                .WithMany(t => t.CashFlows)
                .HasForeignKey(cf => cf.TransactionId);
        }
    }
}