using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class BankAccountTransactionConfiguration : BaseConfiguration<BankAccountTransaction>
    {
        public override void Configure(EntityTypeBuilder<BankAccountTransaction> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.BankAccountId).IsRequired();
            builder.Property(f => f.TransactionId).IsRequired();

            builder.HasOne(bat => bat.Transaction)
                .WithMany(t => t.BankAccountTransactions)
                .HasForeignKey(bat => bat.TransactionId);

            builder.HasOne(bat => bat.BankAccount)
                .WithMany(ba => ba.BankAccountTransactions)
                .HasForeignKey(bat => bat.BankAccountId);
        }
    }
}