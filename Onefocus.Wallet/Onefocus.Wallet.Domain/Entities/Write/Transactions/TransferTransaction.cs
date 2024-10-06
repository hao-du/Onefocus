using Onefocus.Common.Results;

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

    public static Result<TransferTransaction> Create(DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, Enums.Action defaultAction, string description, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, transferredUserID, defaultAction, objectValueDetails);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransferTransaction>(validationResult.Error);
        }

        var transaction = new TransferTransaction(transactedOn, userId, transferredUserID, currencyId, defaultAction, description, actionedBy);

        foreach (var objectValueDetail in objectValueDetails)
        {
            var detailResult = transaction.AddDetail(objectValueDetail);
            if (detailResult.IsFailure)
            {
                return Result.Failure<TransferTransaction>(detailResult.Error);
            }
        }

        return transaction;
    }

    public Result Update(DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, Enums.Action defaultAction, string description, bool activeFlag, Guid actionedBy, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
    {
        var validationResult = Validate(transactedOn, userId, currencyId, transferredUserID, defaultAction, objectValueDetails);
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

    private static Result Validate(DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid transferredUserID, Enums.Action defaultAction, IReadOnlyList<ObjectValues.TransactionDetail> objectValueDetails)
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
        if (!objectValueDetails.Any(ovd => ovd.Action == defaultAction))
        {
            return Result.Failure(Errors.Transaction.Transfer.RequireDefaultActionInDetailList);
        }

        return Result.Success();
    }
}

