using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CashFlowConfiguration : BaseConfiguration<CashFlow>
    {
        public override void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            base.Configure(builder);

            builder.HasOne(bat => bat.Transaction)
                .WithMany(t => t.CashFlows)
                .HasForeignKey(cf => cf.TransactionId);
        }
    }
}