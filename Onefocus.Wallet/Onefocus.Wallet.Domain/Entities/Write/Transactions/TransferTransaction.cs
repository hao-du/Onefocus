using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.Transactions;

public sealed class TransferTransaction : Transaction
{
    public Guid TransferredUserId { get; private set; }

    public User TransferredUser { get; private set; } = default!;

    private TransferTransaction(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, string description, Guid actionedBy) : base(amount, transactedOn, userId, currencyId, description, actionedBy)
    {
        TransferredUserId = transferredUserID;
    }

    public static Result<TransferTransaction> Create(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId, transferredUserID);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransferTransaction>(validationResult.Error);
        }

        return new TransferTransaction(amount, transactedOn, userId, transferredUserID, currencyId, description, actionedBy);
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid transferredUserID, Guid currencyId, string description, bool activeFlag, Guid actionedBy)
    {
        var validationResult = Validate(amount, transactedOn, userId, currencyId, transferredUserID);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = amount;
        TransactedOn = transactedOn;
        UserId = userId;
        TransferredUserId = transferredUserID;
        CurrencyId = currencyId;
        Description = description;

        if (activeFlag) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount, DateTimeOffset transactedOn, Guid userId, Guid currencyId, Guid transferredUserID)
    {
        if (amount < 0)
        {
            return Result.Failure(Errors.Transaction.AmountMustGreaterThanZero);
        }
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

        return Result.Success();
    }
}

