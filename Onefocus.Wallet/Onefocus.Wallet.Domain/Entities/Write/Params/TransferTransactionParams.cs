namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class TransferTransactionParams(Guid? id, decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isInFlow, bool isActive, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        : TransactionParams(id, amount, transactedOn, currencyId, isActive, description, transactionItems)
    {
        public bool IsInFlow { get; private set; } = isInFlow;

        public static TransferTransactionParams Create(Guid? id, decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isInFlow, bool isActive, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        {
            return new TransferTransactionParams(id, amount, transactedOn, currencyId, isInFlow, isActive, description, transactionItems);
        }

        public static TransferTransactionParams CreateNew(decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isInFlow, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        {
            return new TransferTransactionParams(null, amount, transactedOn, currencyId, isInFlow, true, description, transactionItems);
        }
    }
}
