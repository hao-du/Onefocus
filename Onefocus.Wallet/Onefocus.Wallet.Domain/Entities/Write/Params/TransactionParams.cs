namespace Onefocus.Wallet.Domain.Entities.Write.Params
{
    public class TransactionParams(Guid? id, decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isActive, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        public Guid? Id { get; private set; } = id;
        public decimal Amount { get; private set; } = amount;
        public DateTimeOffset TransactedOn { get; private set; } = transactedOn;
        public Guid CurrencyId { get; private set; } = currencyId;
        public string? Description { get; private set; } = description;
        public bool IsActive { get; private set; } = isActive;

        public IReadOnlyList<TransactionItemParams>? TransactionItems = transactionItems;

        public static TransactionParams Create(Guid? id, decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isActive, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        {
            return new TransactionParams(id, amount, transactedOn, currencyId, isActive, description, transactionItems);
        }

        public static TransactionParams CreateNew(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, IReadOnlyList<TransactionItemParams>? transactionItems = null)
        {
            return new TransactionParams(null, amount, transactedOn, currencyId, true, description, transactionItems);
        }
    }
}
