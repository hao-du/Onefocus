using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.Transactions
{
    internal class TransferTransactionConfiguration : IEntityTypeConfiguration<TransferTransaction>
    {
        public void Configure(EntityTypeBuilder<TransferTransaction> builder)
        {
            builder.HasOne(tt => tt.TransferredUser).WithMany(u => u.TransferTransactions).HasForeignKey(tt => tt.TransferredUserId);
        }
    }
}