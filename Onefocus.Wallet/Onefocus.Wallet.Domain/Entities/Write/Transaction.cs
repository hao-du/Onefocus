using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Interfaces;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Transaction : WriteEntityBase, IOwnerUserField
{
    private readonly List<BankAccountTransaction> _bankAccountTransactions = [];
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];
    private readonly List<CurrencyExchangeTransaction> _currencyExchangeTransactions = [];
    private readonly List<CashFlow> _cashFlows = [];
    private readonly List<TransactionItem> _transactionItems = [];

    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; private set; }
    public Guid CurrencyId { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;
    public Currency Currency { get; private set; } = default!;

    public IReadOnlyCollection<BankAccountTransaction> BankAccountTransactions => _bankAccountTransactions.AsReadOnly();
    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchangeTransaction> CurrencyExchangeTransactions => _currencyExchangeTransactions.AsReadOnly();
    public IReadOnlyCollection<CashFlow> CashFlows => _cashFlows.AsReadOnly();
    public IReadOnlyCollection<TransactionItem> TransactionItems => _transactionItems.AsReadOnly();

    private Transaction()
    {
    }

    protected Transaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        TransactedOn = transactedOn;
        OwnerUserId = ownerId;
        CurrencyId = currencyId;
    }

    public static Result<Transaction> Create(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid ownerId, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        var validationResult = Validate(amount, currencyId, transactedOn);
        if (validationResult.IsFailure) return (Result<Transaction>)validationResult;

        var transaction = new Transaction(amount, transactedOn, currencyId, description, ownerId, actionedBy);
        var itemCreationResult = transaction.UpsertTransactionItems(transactionItems, actionedBy);
        if (itemCreationResult.IsFailure) return (Result<Transaction>)itemCreationResult;

        return transaction;
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Guid currencyId, bool isActive, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        var validationResult = Validate(amount, currencyId, transactedOn);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        CurrencyId = currencyId;
        TransactedOn = transactedOn;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return UpsertTransactionItems(transactionItems, actionedBy);
    }

    private Result UpsertTransactionItems(IReadOnlyList<TransactionItemParams>? transactionItems, Guid actionedBy)
    {
        if (transactionItems == null)
        {
            return Result.Success();
        }

        foreach (var item in transactionItems)
        {
            if (item.Id.HasValue)
            {
                var existingItem = _transactionItems.Find(t => t.Id == item.Id);
                if (existingItem == null)
                {
                    return Result.Failure(Errors.TransactionItem.InvalidTransactionItem);
                }

                var updateResult = existingItem.Update(item.Name, item.Amount, item.Description, item.IsActive, actionedBy);
                if (updateResult.IsFailure)
                {
                    return updateResult;
                }
            }
            else
            {
                var itemCreationResult = TransactionItem.Create(item.Name, item.Amount, item.Description, actionedBy);
                if (itemCreationResult.IsFailure)
                {
                    return itemCreationResult;
                }

                _transactionItems.Add(itemCreationResult.Value);
            }
        }

        return Result.Success();
    }

    private static Result Validate(decimal amount, Guid currencyId, DateTimeOffset transactedOn)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (currencyId == default)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (transactedOn == default)
        {
            return Result.Failure(Errors.Transaction.TransactedOnRequired);
        }

        return Result.Success();
    }
}

