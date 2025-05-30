using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class CashFlowConfiguration : IEntityTypeConfiguration<CashFlow>
    {
        public void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            builder.HasMany(cf => cf.Transactions).WithMany(t => t.CashFlows).UsingEntity(e => e.ToTable("CastFlowTransaction"));

            builder.HasQueryFilter(cf => cf.IsActive);
        }
    }
}