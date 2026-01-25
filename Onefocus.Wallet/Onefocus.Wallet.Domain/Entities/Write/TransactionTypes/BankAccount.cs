using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Onefocus.Wallet.Domain.Events.Transaction;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class BankAccount : WriteEntityBase, IAggregateRoot
{
    private readonly List<BankAccountTransaction> _bankAccountTransactions = [];

    public decimal Amount { get; private set; }
    public Guid CurrencyId { get; private set; }
    public decimal InterestRate { get; private set; }
    public string AccountNumber { get; private set; } = default!;

    public DateTimeOffset IssuedOn { get; private set; }
    public DateTimeOffset? ClosedOn { get; private set; }
    public bool IsClosed { get; private set; }
    public Guid BankId { get; private set; }
    public Guid OwnerUserId { get; private set; }

    public User OwnerUser { get; private set; } = default!;
    public Bank Bank { get; private set; } = default!;
    public Currency Currency { get; private set; } = default!;

    public IReadOnlyCollection<BankAccountTransaction> BankAccountTransactions => _bankAccountTransactions.AsReadOnly();

    private BankAccount()
    {
        // Required by EF Core
    }

    private BankAccount(decimal amount, decimal interestRate, Currency currency, string accountNumber, string? description, DateTimeOffset issuedOn, Guid bankId, Guid ownerId, Guid actionedBy)
    {
        Init(description, actionedBy);

        Amount = amount;
        InterestRate = interestRate;
        Currency = currency;
        CurrencyId = currency.Id;
        AccountNumber = accountNumber;
        IssuedOn = issuedOn;
        BankId = bankId;
        OwnerUserId = ownerId;
    }

    public static Result<BankAccount> Create(decimal amount, decimal interestRate, Currency currency, string accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, bool isClosed, Guid bankId, Guid ownerId, Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var validationResult = Validate(amount, currency, bankId, interestRate, issuedOn, isClosed, accountNumber, closedOn);
        if (validationResult.IsFailure) return validationResult.Failure<BankAccount>();

        var bankAccount = new BankAccount(amount, interestRate, currency, accountNumber, description, issuedOn, bankId, ownerId, actionedBy);
        bankAccount.UpdateBankStatus(isClosed, accountNumber, closedOn, actionedBy);

        if (transactionParams.Count > 0)
        {
            var upsertInterestsResult = bankAccount.UpsertInterests(actionedBy, transactionParams);
            if (upsertInterestsResult.IsFailure) return upsertInterestsResult.Failure<BankAccount>();
        }

        bankAccount.AddDomainEvent(BankAccountUpsertedEvent.Create(bankAccount));

        return Result.Success(bankAccount);
    }

    public Result Update(decimal amount, decimal interestRate, Currency currency, string accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset? closedOn, bool isClosed, Guid bankId, bool isActive, Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var validationResult = Validate(amount, currency, bankId, interestRate, issuedOn, isClosed, accountNumber, closedOn);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        InterestRate = interestRate;
        Currency = currency;
        CurrencyId = currency.Id;
        AccountNumber = accountNumber;
        IssuedOn = issuedOn;

        UpdateBankStatus(isClosed, accountNumber, closedOn, actionedBy);

        ClosedOn = closedOn;
        BankId = bankId;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        var upsertInterestsResult = UpsertInterests(actionedBy, transactionParams);
        if (upsertInterestsResult.IsFailure) return upsertInterestsResult;

        var deleteInterestsResult = DeleteInterests(actionedBy, transactionParams);
        if (deleteInterestsResult.IsFailure) return deleteInterestsResult;

        AddDomainEvent(BankAccountUpsertedEvent.Create(this));

        return Result.Success();
    }

    private Result UpsertInterests(Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        foreach (var param in transactionParams)
        {
            var isNew = !param.Id.HasValue || param.Id == Guid.Empty;
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
                    currency: param.Currency,
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

        bankAccountTransaction.SetActiveFlag(param.IsActive, actionedBy);

        var updateResult = bankAccountTransaction.Transaction.Update(param.Amount, param.TransactedOn, param.Currency, param.IsActive, param.Description, actionedBy);
        if (updateResult.IsFailure) return updateResult;

        return Result.Success();
    }

    private Result DeleteInterests(Guid actionedBy, IReadOnlyList<TransactionParams> transactionParams)
    {
        var bankAccountTransactionsToBeDeleted = _bankAccountTransactions.FindAll(t => !t.Id.IsEmpty() && !transactionParams.Any(param => param.Id == t.TransactionId));

        if (bankAccountTransactionsToBeDeleted.Count == 0) return Result.Success();

        foreach (var bankAccountTransaction in bankAccountTransactionsToBeDeleted)
        {
            bankAccountTransaction.SetActiveFlag(false, actionedBy);
            bankAccountTransaction.Transaction.SetActiveFlag(false, actionedBy);
        }

        return Result.Success();
    }

    private Result UpdateBankStatus(bool isClosed, string accountNumber, DateTimeOffset? closedOn, Guid actionedBy)
    {
        var validationResult = ValidateAccountNumber(accountNumber);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        AccountNumber = accountNumber;

        if (isClosed)
        {
            ClosedOn = closedOn;
            IsClosed = true;
        }
        else
        {
            ClosedOn = null;
            IsClosed = false;
        }

        Update(actionedBy);

        return Result.Success();
    }

    public static Result ValidateAccountNumber(string accountNumber)
    {
        if (string.IsNullOrEmpty(accountNumber))
        {
            return Result.Failure(Errors.BankAccount.AccountNumberRequired);
        }

        return Result.Success();
    }

    public static Result Validate(decimal amount, Currency? currency, Guid bankId, decimal interestRate, DateTimeOffset issuedOn, bool isClosed, string accountNumber, DateTimeOffset? closedOn)
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
        if (bankId == default)
        {
            return Result.Failure(Errors.Bank.BankRequired);
        }
        if (interestRate < (decimal)0.01)
        {
            return Result.Failure(Errors.BankAccount.InterestRateMustEqualOrGreaterThanZero);
        }
        if (interestRate > 100)
        {
            return Result.Failure(Errors.BankAccount.InterestRateMustEqualOrLessThanOneHundred);
        }
        if (issuedOn == default)
        {
            return Result.Failure(Errors.BankAccount.IssuedOnRequired);
        }
        if (isClosed)
        {
            var accountNumberValidationResult = ValidateAccountNumber(accountNumber);
            if (accountNumberValidationResult.IsFailure)
            {
                return accountNumberValidationResult;
            }
            if (!closedOn.HasValue || closedOn.Value == default)
            {
                return Result.Failure(Errors.BankAccount.ClosedOnRequired);
            }
        }

        return Result.Success();
    }
}

