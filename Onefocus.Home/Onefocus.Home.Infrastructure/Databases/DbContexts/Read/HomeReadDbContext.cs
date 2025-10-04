using Microsoft.EntityFrameworkCore;
using Onefocus.Home.Domain.Entities.Read;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Read.Configurations;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Read;

public class HomeReadDbContext(DbContextOptions<HomeReadDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Settings> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new SettingConfiguration());
    }
}
