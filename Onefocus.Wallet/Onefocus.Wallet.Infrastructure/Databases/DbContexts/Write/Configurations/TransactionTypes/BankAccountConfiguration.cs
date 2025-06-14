using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class BankAccountConfiguration : BaseConfiguration<BankAccount>
    {
        public override void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            base.Configure(builder);

            builder.Property(f => f.AccountNumber).HasMaxLength(50).IsRequired();
            builder.Property(f => f.Amount).HasPrecision(18, 2).IsRequired();
            builder.Property(f => f.InterestRate).HasPrecision(18, 2).IsRequired();
            builder.Property(f => f.CurrencyId).IsRequired();
            builder.Property(f => f.IssuedOn).IsRequired();
            builder.Property(f => f.IsClosed).IsRequired();
            builder.Property(f => f.BankId).IsRequired();
            builder.Property(f => f.OwnerUserId).IsRequired();

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
