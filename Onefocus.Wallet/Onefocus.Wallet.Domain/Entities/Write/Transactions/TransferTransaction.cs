using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Models;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class TransferTransaction : Transaction
{
    public Guid TransferredUserId { get; private set; }
    public User TransferredUser { get; private set; } = default!;
    public Enums.Action DefaultAction { get; private set; }

    private TransferTransaction() : base()
    {
    }

    private TransferTransaction(DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, Enums.Action defaultAction, string description, Guid actionedBy) : base(transactedOn, userId, currencyId, description, actionedBy)
    {
        TransferredUserId = transferredUserID;
        DefaultAction = defaultAction;
    }

    public static Result<TransferTransaction> Create(DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, Enums.Action defaultAction, string description, Guid actionedBy, IReadOnlyList<TransactionDetailParams> details)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, transferredUserID, defaultAction, details);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransferTransaction>(validationResult.Error);
        }

        var transaction = new TransferTransaction(transactedOn, userId, transferredUserID, currencyId, defaultAction, description, actionedBy);

        foreach (var objectValueDetail in details)
        {
            var detailResult = transaction.AddDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<TransferTransaction>(detailResult.Error);
            }
        }

        return transaction;
    }

    public Result Update(DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, Enums.Action defaultAction, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<TransactionDetailParams> details)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, transferredUserID, defaultAction, details);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        TransactedOn = transactedOn;
        UserId = userId;
        TransferredUserId = transferredUserID;
        DefaultAction = defaultAction;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        foreach (var objectValueDetail in details)
        {
            var detailResult = UpsertDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure(detailResult.Error);
            }
        }

        return Result.Success();
    }

    private static Result Validate(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid transferredUserID, Enums.Action defaultAction, IReadOnlyList<TransactionDetailParams> details)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure(Errors.User.UserRequired);
        }
        if (transferredUserID == Guid.Empty)
        {
            return Result.Failure<TransferTransaction>(Errors.Transaction.Transfer.TransferredUserRequired);
        }
        if (currencyId == Guid.Empty)
        {
            return Result.Failure(Errors.Currency.CurrencyRequired);
        }
        if (!details.Any(d => d.Action == defaultAction))
        {
            return Result.Failure(Errors.Transaction.Transfer.RequireDefaultActionInDetailList);
        }

        return Result.Success();
    }
}

