using Microsoft.EntityFrameworkCore;
using Onefocus.Home.Domain.Entities.Read;
using Onefocus.Home.Domain.Entities.Write;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Write.Configurations;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Write;

public class HomeWriteDbContext(DbContextOptions<HomeWriteDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Setting> Setting { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new CounterpartyConfiguration());
    }
}

