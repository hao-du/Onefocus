using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Write;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).IsRequired();
            builder.Property(u => u.SettingId).IsRequired(false);

            builder.HasOne(u => u.Setting)
                .WithOne(s => s.User)
                .HasForeignKey<User>(u => u.SettingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}