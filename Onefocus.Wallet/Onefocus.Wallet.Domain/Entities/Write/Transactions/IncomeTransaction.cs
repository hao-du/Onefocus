using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class IncomeTransaction : Transaction
{
    private IncomeTransaction() : base()
    {
    }

    private IncomeTransaction(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy) : base(transactedOn, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<IncomeTransaction> Create(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<IncomeTransaction>(validationResult.Error);
        }

        var transaction = new IncomeTransaction(transactedOn, userId, currencyId, description, actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = transaction.AddDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<IncomeTransaction>(detailResult.Error);
            }
        }

        transaction.CalculateAmount();

        return transaction;
    }

    public Result Update(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        TransactedOn = transactedOn;
        UserId = userId;
        CurrencyId = currencyId;
        Description = description;

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

        CalculateAmount();

        return Result.Success();
    }

    private static Result Validate(DateTimeOffset transactedOn, Guid userId, Guid currencyId)
    {
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

