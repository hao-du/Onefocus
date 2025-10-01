using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class SettingConfiguration : BaseConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            builder.Property(t => t.UserId).IsRequired();

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