using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations;

internal class OutboxEventConfiguration : BaseConfiguration<OutboxEvent>
{
    public override void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        base.Configure(builder);
        builder.Property(o => o.VectorSearchTerms)
            .HasColumnType("jsonb")
            .HasConversion(
                t => JsonHelper.SerializeJson(t),
                t => JsonHelper.DeserializeJson<Dictionary<string, string>>(t)
            );
    }
}
