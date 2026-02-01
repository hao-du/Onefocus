using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Utilities;
using Onefocus.Search.Domain.Entities;

namespace Onefocus.Search.Infrastructure.Databases.DbContexts.Configurations;

internal class SearchIndexQueueConfiguration : BaseConfiguration<SearchIndexQueue>
{
    public override void Configure(EntityTypeBuilder<SearchIndexQueue> builder)
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
