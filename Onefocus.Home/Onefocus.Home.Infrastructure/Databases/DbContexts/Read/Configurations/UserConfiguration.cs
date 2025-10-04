using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Home.Domain.Entities.Read;

namespace Onefocus.Home.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasOne(u => u.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<Settings>(u => u.UserId)
                .IsRequired();
        }
    }
}