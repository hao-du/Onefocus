using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class BankAccount : WriteEntityBase, IAggregateRoot
{
    private readonly List<BankAccountTransaction> _bankAccountTransactions = [];

    public decimal Amount { get; private set; }
    public Guid CurrencyId { get; private set; }
    public decimal? InterestRate { get; private set; }
    public string? AccountNumber { get; private set; } = default!;

    public DateTimeOffset IssuedOn { get; private set; }
    public DateTimeOffset? ClosedOn { get; private set; }
    public bool CloseFlag { get; private set; }
    public Guid BankId { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;
    public Bank Bank { get; private set; } = default!;
    public Currency Currency { get; private set; } = default!;

    public IReadOnlyCollection<BankAccountTransaction> BankAccountTransactions => _bankAccountTransactions.AsReadOnly();


    private BankAccount()
    {
        AccountNumber = default!;
    }

    public BankAccount(decimal amount, decimal? interestRate, Guid currencyId, string? accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, bool closeFlag, Guid bankId, Guid ownerId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        InterestRate = interestRate;
        CurrencyId = currencyId;
        AccountNumber = accountNumber;
        IssuedOn = issuedOn;
        ClosedOn = closedOn;
        CloseFlag = closeFlag;
        BankId = bankId;
        OwnerUserId = ownerId;
    }

    public static Result<BankAccount> Create(decimal amount, decimal? interestRate, Guid currencyId, string? accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, bool closeFlag, Guid bankId, Guid ownerId, Guid actionedBy)
    {
        var validationResult = Validate(amount, currencyId, issuedOn);
        if (validationResult.IsFailure)
        {
            return Result.Failure<BankAccount>(validationResult.Errors);
        }

        return new BankAccount(amount, interestRate, currencyId, accountNumber, description, issuedOn, closedOn, closeFlag, bankId, ownerId, actionedBy);
    }

    public Result CreateInterest(decimal interest, DateTimeOffset transactedOn, string? description, Guid actionedBy)
    {
        var createTransactionResult = Transaction.Create(interest, transactedOn, CurrencyId, description, OwnerUserId, actionedBy);
        if (createTransactionResult.IsFailure)
        {
            return createTransactionResult;
        }

        var bankAccountTransaction = BankAccountTransaction.Create(createTransactionResult.Value);
        _bankAccountTransactions.Add(bankAccountTransaction);

        return Result.Success();
    }

    public Result Update(decimal amount, decimal? interestRate, Guid currencyId, string? accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, Guid bankId, bool isActive, Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var validationResult = Validate(amount, currencyId, issuedOn);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        InterestRate = interestRate;
        CurrencyId = currencyId;
        AccountNumber = accountNumber;
        IssuedOn = issuedOn;
        ClosedOn = closedOn;
        BankId = bankId;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        var bankAccountTransactions = _bankAccountTransactions
            .Where(bat => transactionParams.Any(param => param.Id == bat.TransactionId))
            .ToList();
        foreach (var bankAccountTransaction in bankAccountTransactions)
        {
            if (isActive) bankAccountTransaction.MarkActive(actionedBy);
            else bankAccountTransaction.MarkInactive(actionedBy);
        }
        foreach (var transaction in bankAccountTransactions.Select(bat => bat.Transaction))
        {
            var param = transactionParams.FirstOrDefault(param => param.Id == transaction.Id);
            if (param == null)
            {
                return Result.Failure(Errors.Transaction.InvalidTransaction);
            }

            var upsertResult = transaction.Update(param.Amount, param.TransactedOn, param.CurrencyId, param.IsActive, param.Description, actionedBy);
            if (upsertResult.IsFailure)
            {
                return upsertResult;
            }
        }

        return Result.Success();
    }

    public Result CloseBankAccount(string accountNumber, DateTimeOffset closedOn, Guid actionedBy)
    {
        if (closedOn == default)
        {
            return Result.Failure(Errors.BankAccount.ClosedOnRequired);
        }
        if (string.IsNullOrEmpty(accountNumber))
        {
            return Result.Failure(Errors.BankAccount.AccountNumberRequired);
        }
        if (closedOn.UtcDateTime.Date > DateTimeOffset.UtcNow.Date)
        {
            return Result.Failure(Errors.Transaction.ClosedOnCannotBeAFutureDate);
        }

        AccountNumber = accountNumber;
        ClosedOn = closedOn;
        CloseFlag = true;
        Update(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, Guid currencyId, DateTimeOffset issuedOn)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustEqualOrGreaterThanZero);
        }
        if (currencyId == default)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (issuedOn == default)
        {
            return Result.Failure(Errors.BankAccount.IssuedOnRequired);
        }

        return Result.Success();
    }
}

