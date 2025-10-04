using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Read;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class SettingConfiguration : BaseConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
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