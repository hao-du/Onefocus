using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Read;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class SettingConfiguration : BaseConfiguration<Settings>
    {
        public override void Configure(EntityTypeBuilder<Settings> builder)
        {
            base.Configure(builder);

            builder.HasIndex(s => s.UserId).IsUnique();

            builder.OwnsOne(s => s.Preferences, pref =>
            {
                pref.ToJson();
            }); 
        }
    }
}