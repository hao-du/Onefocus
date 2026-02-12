using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Utilities;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : WriteEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(t => t.Id).HasValueGenerator<IdGenerator>().ValueGeneratedOnAdd();

        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(255);

        builder.HasIndex(t => new { t.CreatedOn })
            .HasDatabaseName($"IX_{typeof(T).Name}_CreatedOn");
    }
}

