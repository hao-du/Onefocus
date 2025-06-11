using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class BankAccountTransactionConfiguration : IEntityTypeConfiguration<BankAccountTransaction>
    {
        public void Configure(EntityTypeBuilder<BankAccountTransaction> builder)
        {
            builder.HasOne(bat => bat.Transaction)
                .WithMany(t => t.BankAccountTransactions)
                .HasForeignKey(bat => bat.TransactionId);

            builder.HasOne(bat => bat.BankAccount)
                .WithMany(ba => ba.BankAccountTransactions)
                .HasForeignKey(bat => bat.BankAccountId);

            builder.HasQueryFilter(bat => bat.IsActive);
        }
    }
}