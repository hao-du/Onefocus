using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public class OutcomeTransaction : Transaction
{
    private OutcomeTransaction(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy) : base(amount, transactedOn, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<OutcomeTransaction> Create(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<OutcomeTransaction>(validationResult.Error);
        }

        return new OutcomeTransaction(amount, transactedOn, userId, currencyId, description, actionedBy);
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId)
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

        return Result.Success();
    }
}

