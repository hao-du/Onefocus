using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.ObjectValues;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class BankingTransaction : Transaction
{
    public BankAccount BankAccount { get; private set; }

    private BankingTransaction(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, BankAccount bankAccount, string description, Guid actionedBy) : base(amount, transactedOn, userId, currencyId, description, actionedBy)
    {
        BankAccount = bankAccount;
    }

    public static Result<BankingTransaction> Create(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, BankAccount bankAccount, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId, bankAccount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<BankingTransaction>(validationResult.Error);
        }

        return new BankingTransaction(amount, transactedOn, userId, currencyId, bankAccount, description, actionedBy);
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, BankAccount bankAccount, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId, bankAccount);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        BankAccount.Update(bankAccount.AccountNumber, bankAccount.BankId, bankAccount.ClosedOn, bankAccount.CloseFlag);

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    public void CloseBankAccount(Guid actionedBy)
    {
        BankAccount.SetCloseFlag(true);
        Update(actionedBy);
    }

    private static Result Validate(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, BankAccount bankAccount)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (bankAccount == null) 
        {
            return Result.Failure(CommonErrors.NullReference);
        }


        return Result.Success();
    }
}

