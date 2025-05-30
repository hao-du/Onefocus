using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes
{
    public abstract class BaseTransaction: ReadEntityBase
    {
        protected List<Transaction> _transactions = [];
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    }
}
