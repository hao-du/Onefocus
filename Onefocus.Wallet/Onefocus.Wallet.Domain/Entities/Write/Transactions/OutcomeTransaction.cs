using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Models;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class OutcomeTransaction : Transaction
{
    private OutcomeTransaction() : base()
    {
    }

    private OutcomeTransaction(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy) : base(transactedOn, userId, currencyId, description, actionedBy)
    {
    }

    public static Result<OutcomeTransaction> Create(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, Guid actionedBy, IReadOnlyList<TransactionDetailParams> details)
    {
        var validationResult = Validate(transactedOn, userId, currencyId);
        if (validationResult.IsFailure)
        {
            return Result.Failure<OutcomeTransaction>(validationResult.Error);
        }

        var transaction = new OutcomeTransaction(transactedOn, userId, currencyId, description, actionedBy);

        foreach (var detail in details)
        {
            var detailResult = transaction.AddDetail(detail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<OutcomeTransaction>(detailResult.Error);
            }
        }

        return transaction;
    }

    public Result Update(DateTimeOffset transactedOn, Guid userId, Guid currencyId, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<TransactionDetailParams> details)
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

        foreach (var detail in details)
        {
            var detailResult = UpsertDetail(detail);
            if (detailResult.IsFailure)
            {
                return Result.Failure(detailResult.Error);
            }
        }

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

