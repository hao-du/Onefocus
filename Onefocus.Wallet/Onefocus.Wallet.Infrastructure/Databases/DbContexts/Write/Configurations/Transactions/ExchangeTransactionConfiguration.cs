using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write.Configurations.Transactions
{
    internal class ExchangeTransactionConfiguration : IEntityTypeConfiguration<ExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<ExchangeTransaction> builder)
        {
            builder.HasOne(et => et.ExchangedCurrency).WithMany(c => c.ExchangeTransactions).HasForeignKey(et => et.ExchangedCurrencyId);
        }
    }
}