using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50);
            builder.Property(p => p.Email).HasMaxLength(256);
            builder.Property(p => p.Description).HasMaxLength(255);

            builder.HasMany(u => u.TransferTransactions).WithOne(tu => tu.TransferredUser).HasForeignKey(tu => tu.TransferredUserId);
            builder.HasMany(u => u.Transactions).WithOne(tu => tu.User).HasForeignKey(tu => tu.UserId);
        }
    }
}