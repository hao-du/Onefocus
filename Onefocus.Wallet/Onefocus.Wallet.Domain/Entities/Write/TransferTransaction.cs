using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class TransferTransaction : Transaction
{
    private TransferTransaction(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, Guid actionedBy): base(amount, traceDate, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<TransferTransaction> Create(decimal amount, DateTimeOffset traceDate, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        if(amount < 0)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Currency.CurrencyRequired);
        }

        return new TransferTransaction(amount, traceDate, userId, currencyId, description, actionedBy);
    }

    public Result<TransferTransaction> Update(decimal amount, DateTimeOffset date, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.AmountMustGreaterThanZero);
        }
        if (userId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.User.UserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Currency.CurrencyRequired);
        }

        Amount = amount;
        Date = date;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }

    public void SetActiveFlag(bool activeFlag, Guid actionedBy)
    {
        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);
    }
}

