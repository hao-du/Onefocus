using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.Transactions
{
    internal class BankingTransactionConfiguration : IEntityTypeConfiguration<BankingTransaction>
    {
        public void Configure(EntityTypeBuilder<BankingTransaction> builder)
        {
            builder.ComplexProperty(bt => bt.BankAccount, ba =>
            {
                ba.Property(p => p.AccountNumber).HasMaxLength(50);
            });

            builder.HasOne(bt => bt.BankAccount.Bank).WithMany(b => b.BankingTransactions).HasForeignKey(bt => bt.BankAccount.BankId);
        }
    }
}
