using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class SettingConfiguration : BaseConfiguration<Settings>
    {
        public override void Configure(EntityTypeBuilder<Settings> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.UserId).IsRequired();

            builder.HasIndex(s => s.UserId).IsUnique();

            builder.OwnsOne(s => s.Preferences, pref =>
            {
                pref.ToJson();
            }); 
        }
    }
}