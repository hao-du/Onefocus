using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class CashFlowConfiguration : IEntityTypeConfiguration<CashFlow>
    {
        public void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            builder.Property(cf => cf.Description).HasMaxLength(255);

            builder.HasMany(cf => cf.Transactions).WithMany(t => t.CashFlows).UsingEntity(e => e.ToTable("CastFlowTransaction"));

            builder.HasQueryFilter(cf => cf.IsActive);
        }
    }
}