using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.ObjectValues;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class BankingTransaction : Transaction
{
    public BankAccount BankAccount { get; private set; }
    public Guid BankId { get; private set; }
    public Bank Bank { get; private set; } = default!;
    public decimal WithdrawalAmount => TransactionDetails.Where(td => td.Action == Enums.Action.Withdraw).Sum(td => td.Amount);
    public decimal InterestAmount => TransactionDetails.Where(td => td.Action == Enums.Action.GetInterest).Sum(td => td.Amount);

    private BankingTransaction(): base()
    {
        BankAccount = default!;
    }

    private BankingTransaction(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid bankId, BankAccount bankAccount, string description, Guid actionedBy) : base(transactedOn, userId, currencyId, description, actionedBy)
    {
        BankAccount = bankAccount;
        BankId = bankId;
    }

    public static Result<BankingTransaction> Create(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid bankId, BankAccount bankAccount, string description, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, bankId, bankAccount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<BankingTransaction>(validationResult.Error);
        }

        var transaction = new BankingTransaction(transactedOn, userId, currencyId, bankId, bankAccount, description, actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = transaction.AddDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<BankingTransaction>(detailResult.Error);
            }
        }

        return transaction;
    }

    public Result Update(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid bankId, BankAccount bankAccount, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, bankId, bankAccount);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        TransactedOn = transactedOn;
        BankId = bankId;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        BankAccount.Update(bankAccount.AccountNumber, bankAccount.ClosedOn, bankAccount.CloseFlag);

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = UpsertDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure(detailResult.Error);
            }
        }

        return Result.Success();
    }

    public void CloseBankAccount(Guid actionedBy)
    {
        BankAccount.SetCloseFlag(true);
        Update(actionedBy);
    }

    private static Result Validate(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid bankId, BankAccount bankAccount)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (bankId == Guid.Empty)
        {
            return Result.Failure<BankAccount>(Errors.Bank.BankRequired);
        }
        if (bankAccount == null) 
        {
            return Result.Failure(CommonErrors.NullReference);
        }

        return Result.Success();
    }
}

