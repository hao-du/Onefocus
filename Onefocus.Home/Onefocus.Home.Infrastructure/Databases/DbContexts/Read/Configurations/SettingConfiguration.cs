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

            builder.HasOne(s => s.User)
                .WithOne(u => u.Setting)
                .HasForeignKey<Setting>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.OwnsOne(s => s.Preference, pref =>
            {
                pref.ToJson();
            }); 
        }
    }
}