﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Write;
internal abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : WriteEntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(f => f.IsActive).IsRequired();
        builder.Property(f => f.Description).HasMaxLength(255);

        builder.HasQueryFilter(ba => ba.IsActive);
    }
}

