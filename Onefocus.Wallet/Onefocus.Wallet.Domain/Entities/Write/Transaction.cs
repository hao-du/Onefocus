using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class Transaction : WriteEntityBase
{
    private readonly List<BankAccount> _bankAccounts = [];
    private readonly List<PeerTransfer> _peerTransfers = [];
    private readonly List<CurrencyExchange> _currencyExchanges = [];
    private readonly List<CashFlow> _cashFlows = [];
    private readonly List<TransactionItem> _transactionItems = [];

    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Guid UserId { get; protected set; }
    public Guid CurrencyId { get; protected set; }

    public User User { get; protected set; } = default!;
    public Currency Currency { get; protected set; } = default!;

    public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();
    public IReadOnlyCollection<CurrencyExchange> CurrencyExchanges => _currencyExchanges.AsReadOnly();
    public IReadOnlyCollection<CashFlow> CashFlows => _cashFlows.AsReadOnly();
    public IReadOnlyCollection<TransactionItem> TransactionItems => _transactionItems.AsReadOnly();

    private Transaction()
    {
    }

    protected Transaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = actionedBy;
        CurrencyId = currencyId;
    }

    public static Result<Transaction> Create(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        var validationResult = Validate(amount, currencyId, transactedOn);
        if (validationResult.IsFailure)
        {
            return Result.Failure<Transaction>(validationResult.Error);
        }

        var transaction = new Transaction(amount, transactedOn, currencyId, description, actionedBy);
        var itemCreationResult  = transaction.UpsertTransactionItems(transactionItems, actionedBy);
        if (itemCreationResult.IsFailure)
        {
            return Result.Failure<Transaction>(itemCreationResult.Error);
        }

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

