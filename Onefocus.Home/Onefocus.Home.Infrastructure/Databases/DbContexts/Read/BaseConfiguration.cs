using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Read;
internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : ReadEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasQueryFilter(ba => ba.IsActive);
    }
}

