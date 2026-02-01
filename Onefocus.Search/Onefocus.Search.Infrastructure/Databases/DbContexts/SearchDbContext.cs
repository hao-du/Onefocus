using Microsoft.EntityFrameworkCore;
using Onefocus.Search.Domain.Entities;
using Onefocus.Search.Infrastructure.Databases.DbContexts.Configurations;

namespace Onefocus.Search.Infrastructure.Databases.DbContexts;

public class SearchDbContext(DbContextOptions<SearchDbContext> options) : DbContext(options)
{
    public required DbSet<SearchIndexQueue> SearchIndexQueue { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new SearchIndexQueueConfiguration());
    }
}

