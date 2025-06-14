using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.TransactionTypes
{
    internal class BankAccountConfiguration : BaseConfiguration<BankAccount>
    {
        public override void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            base.Configure(builder);

            builder.HasOne(ba => ba.OwnerUser)
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(ba => ba.OwnerUserId);

            builder.HasOne(ba => ba.Bank)
                .WithMany(b => b.BankAccounts)
                .HasForeignKey(ba => ba.BankId);

            builder.HasOne(ba => ba.Currency)
                .WithMany(c => c.BankAccounts)
                .HasForeignKey(ba => ba.CurrencyId);

            builder.HasMany(ba => ba.BankAccountTransactions)
                .WithOne(bat => bat.BankAccount)
                .HasForeignKey(bat => bat.BankAccountId);
        }
    }
}
