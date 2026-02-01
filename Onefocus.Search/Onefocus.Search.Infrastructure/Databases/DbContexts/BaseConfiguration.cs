using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Utilities;

namespace Onefocus.Search.Infrastructure.Databases.DbContexts;
internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : WriteEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.Id).HasValueGenerator<IdGenerator>().ValueGeneratedOnAdd();

        builder.Property(f => f.IsActive).IsRequired();
        builder.Property(f => f.Description).HasMaxLength(255);

        builder.HasQueryFilter(ba => ba.IsActive);
    }
}

