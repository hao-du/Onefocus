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

    public static Result<BankAccount> Create(decimal amount, decimal? interestRate, Guid currencyId, string? accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, bool closeFlag, Guid bankId, Guid ownerId, Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var validationResult = Validate(amount, currencyId, issuedOn);
        if (validationResult.IsFailure) return (Result<BankAccount>)validationResult;

        var bankAccount = new BankAccount(amount, interestRate, currencyId, accountNumber, description, issuedOn, closedOn, closeFlag, bankId, ownerId, actionedBy);

        var upsertInterestsResult = bankAccount.UpsertInterests(actionedBy, transactionParams);
        if (upsertInterestsResult.IsFailure) return (Result<BankAccount>)upsertInterestsResult;

        return Result.Success(bankAccount);
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

        var upsertInterestsResult = UpsertInterests(actionedBy, transactionParams);
        if (upsertInterestsResult.IsFailure) return upsertInterestsResult;

        return Result.Success();
    }

    private Result UpsertInterests(Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        foreach (var param in transactionParams)
        {
            var isNew = param.Id.HasValue && param.Id != Guid.Empty;
            if (isNew)
            {
                var createDetailResult = CreateInterests(actionedBy, param);
                if (createDetailResult.IsFailure) return createDetailResult;
            }
            else
            {
                var updateDetailResult = UpdateInterests(actionedBy, param);
                if (updateDetailResult.IsFailure) return updateDetailResult;
            }
        }

        return Result.Success();
    }

    private Result CreateInterests(Guid actionedBy, TransactionParams param)
    {
        var createTransactionResult = Transaction.Create(
                    amount: param.Amount,
                    transactedOn: param.TransactedOn,
                    currencyId: param.CurrencyId,
                    description: param.Description,
                    ownerId: OwnerUserId,
                    actionedBy: actionedBy,
                    transactionItems: param.TransactionItems
                );
        if (createTransactionResult.IsFailure) return createTransactionResult;

        var createBankAccountTransactionResult = BankAccountTransaction.Create(createTransactionResult.Value);
        if (createBankAccountTransactionResult.IsFailure) return createBankAccountTransactionResult;

        _bankAccountTransactions.Add(createBankAccountTransactionResult.Value);

        return Result.Success();
    }

    private Result UpdateInterests(Guid actionedBy, TransactionParams param)
    {
        var bankAccountTransaction = _bankAccountTransactions.Find(bat => bat.TransactionId == param.Id);
        if (bankAccountTransaction == null)
        {
            return Result.Failure(Errors.Transaction.InvalidTransaction);
        }

        if (param.IsActive) bankAccountTransaction.MarkActive(actionedBy);
        else bankAccountTransaction.MarkInactive(actionedBy);

        var updateResult = bankAccountTransaction.Transaction.Update(param.Amount, param.TransactedOn, param.CurrencyId, param.IsActive, param.Description, actionedBy);
        if (updateResult.IsFailure) return updateResult;

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

