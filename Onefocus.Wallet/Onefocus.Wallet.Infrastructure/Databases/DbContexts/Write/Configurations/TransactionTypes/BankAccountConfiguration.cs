using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.TransactionTypes
{
    internal class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(ba => ba.AccountNumber).HasMaxLength(50);
            builder.Property(ba => ba.Description).HasMaxLength(255);

            builder.HasOne(ba => ba.Bank).WithMany(b => b.BankAccounts).HasForeignKey(ba => ba.BankId);
            builder.HasOne(ba => ba.Currency).WithMany(c => c.BankAccounts).HasForeignKey(ba => ba.CurrencyId);
            builder.HasMany(ba => ba.Transactions).WithMany(t => t.BankAccounts).UsingEntity(e => e.ToTable("BankAccountTransaction"));

            builder.HasQueryFilter(ba => ba.IsActive);
        }
    }
}
