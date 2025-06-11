using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CashFlowConfiguration : IEntityTypeConfiguration<CashFlow>
    {
        public void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            builder.HasOne(bat => bat.Transaction)
                .WithMany(t => t.CashFlows)
                .HasForeignKey(cf => cf.TransactionId);

            builder.HasQueryFilter(cf => cf.IsActive);
        }
    }
}