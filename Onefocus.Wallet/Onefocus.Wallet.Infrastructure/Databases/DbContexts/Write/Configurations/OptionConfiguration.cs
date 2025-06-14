using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class OptionConfiguration : BaseConfiguration<Option>
    {
        public override void Configure(EntityTypeBuilder<Option> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
            builder.Property(c => c.OptionType).IsRequired();
            builder.Property(c => c.OwnerUserId).IsRequired();

            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Options)
                .HasForeignKey(c => c.OwnerUserId);
        }
    }
}
