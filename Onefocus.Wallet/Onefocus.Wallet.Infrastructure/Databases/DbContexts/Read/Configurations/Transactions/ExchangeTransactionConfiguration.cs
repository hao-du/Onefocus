using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read.Configurations.Transactions
{
    internal class ExchangeTransactionConfiguration : IEntityTypeConfiguration<ExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<ExchangeTransaction> builder)
        {
            builder.HasOne(et => et.ExchangedCurrency).WithMany(c => c.ExchangeTransactions).HasForeignKey(et => et.ExchangedCurrencyId);
        }
    }
}