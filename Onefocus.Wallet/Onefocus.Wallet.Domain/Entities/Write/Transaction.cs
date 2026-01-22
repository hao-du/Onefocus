using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
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
        // Required for EF Core
    }

    private Transaction(decimal amount, DateTimeOffset transactedOn, Currency currency, string? description, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.CreateVersion7(), description, actionedBy);

        Amount = amount;
        TransactedOn = transactedOn;
        OwnerUserId = ownerId;
        Currency = currency;
        CurrencyId = currency.Id;
    }

    public static Result<Transaction> Create(decimal amount, DateTimeOffset transactedOn, Currency currency, string? description, Guid ownerId, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        var validationResult = Validate(amount, currency, transactedOn);
        if (validationResult.IsFailure) return (Result<Transaction>)validationResult;

        var transaction = new Transaction(amount, transactedOn, currency, description, ownerId, actionedBy);
        var itemCreationResult = transaction.UpsertTransactionItems(transactionItems, actionedBy);
        if (itemCreationResult.IsFailure) return (Result<Transaction>)itemCreationResult;

        return transaction;
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Currency currency, bool isActive, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        var validationResult = Validate(amount, currency, transactedOn);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        Currency = currency;
        CurrencyId = currency.Id;
        TransactedOn = transactedOn;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        var upsertTransactionItemsResult = UpsertTransactionItems(transactionItems, actionedBy);
        if (upsertTransactionItemsResult.IsFailure) return upsertTransactionItemsResult;

        var deleteTransactionItemsResult = DeleteTransactionItems(transactionItems, actionedBy);
        if (deleteTransactionItemsResult.IsFailure) return deleteTransactionItemsResult;

        return Result.Success();
    }

    private Result UpsertTransactionItems(IReadOnlyList<TransactionItemParams>? transactionItems, Guid actionedBy)
    {
        if (transactionItems == null)
        {
            return Result.Success();
        }

        foreach (var item in transactionItems)
        {
            if (item.Id.HasValue && item.Id != Guid.Empty)
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

    private Result DeleteTransactionItems(IReadOnlyList<TransactionItemParams>? transactionItems, Guid actionedBy)
    {
        if (transactionItems == null)
        {
            return Result.Success();
        }

        var transactionItemsToBeDeleted = _transactionItems.FindAll(t => !t.Id.IsEmpty() && !transactionItems.Any(param => param.Id == t.Id));

        if (transactionItemsToBeDeleted.Count == 0) return Result.Success();

        foreach (var item in transactionItemsToBeDeleted)
        {
            item.SetActiveFlag(false, actionedBy);
        }

        return Result.Success();
    }

    public static Result Validate(decimal amount, Currency? currency, DateTimeOffset transactedOn)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (amount > 10000000000)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrLessThanTenBillion);
        }
        if (currency == null)
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

