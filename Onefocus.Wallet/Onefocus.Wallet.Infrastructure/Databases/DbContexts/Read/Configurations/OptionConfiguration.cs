using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations
{
    internal class OptionConfiguration : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasOne(c => c.OwnerUser)
                .WithMany(u => u.Options)
                .HasForeignKey(c => c.OwnerUserId);

            builder.HasQueryFilter(c => c.IsActive);
        }
    }
}
