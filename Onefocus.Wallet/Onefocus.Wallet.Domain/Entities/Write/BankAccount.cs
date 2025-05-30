using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class BankAccount : WriteEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();

    public decimal Amount { get; private set; }
    public Guid CurrencyId { get; private set; }
    public decimal? InterestRate { get; private set; }
    public string AccountNumber { get; private set; }

    public DateTimeOffset IssuedOn { get; private set; }
    public DateTimeOffset ClosedOn { get; private set; }
    public bool CloseFlag { get; private set; } = false;
    public Guid BankId { get; private set; }

    public Bank Bank { get; private set; } = default!;
    public Currency Currency { get; private set; } = default!;
    public IReadOnlyCollection<Transaction> TransactionDetails => _transactions.AsReadOnly();


    private BankAccount()
    {
        AccountNumber = default!;
    }

    public BankAccount(decimal amount, decimal? interestRate, Guid currencyId, string accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset closedOn, Guid bankId, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Amount = amount;
        InterestRate = interestRate;
        CurrencyId = currencyId;
        AccountNumber = accountNumber;
        IssuedOn = issuedOn;
        ClosedOn = closedOn;
        BankId = bankId;
    }

    public static Result<BankAccount> Create(decimal amount, decimal? interestRate, Guid currencyId, string accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset closedOn, Guid bankId, Guid actionedBy)
    {
        var validationResult = Validate(amount, currencyId, issuedOn);
        if (validationResult.IsFailure)
        {
            return Result.Failure<BankAccount>(validationResult.Error);
        }

        return new BankAccount(amount, interestRate, currencyId, accountNumber, description, issuedOn, closedOn, bankId, actionedBy);
    }

    public Result Update(decimal amount, decimal? interestRate, Guid currencyId, string accountNumber, string? description, DateTimeOffset issuedOn, DateTimeOffset closedOn, Guid bankId, bool isActive, Guid actionedBy)
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

