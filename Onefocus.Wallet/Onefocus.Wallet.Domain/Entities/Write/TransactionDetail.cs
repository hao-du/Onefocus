using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class TransactionDetail: WriteEntityBase
{
    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; private set; }
    public Enums.Action Action { get; private set; }
    public Guid TransactionId { get; private set; }

    public Transaction Transaction { get; private set; } = default!;

    private TransactionDetail(Guid transactionId, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, Guid actionedBy)
    {
        TransactionId = transactionId;
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;

        Init(Guid.NewGuid(), description, actionedBy);
    }

    internal static Result<TransactionDetail> Create(ObjectValues.TransactionDetail objectValue)
    {
        var validationResult = Validate(objectValue.Amount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransactionDetail>(validationResult.Error);
        }

        return new TransactionDetail(objectValue.TransactionId, objectValue.Amount, objectValue.TransactedOn, objectValue.Action, objectValue.Description, objectValue.ActionedBy);
    }

    internal Result Update(ObjectValues.TransactionDetail objectValue)
    {
        var validationResult = Validate(objectValue.Amount);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = objectValue.Amount;
        TransactedOn = objectValue.TransactedOn;
        Action = objectValue.Action;
        Description = objectValue.Description;

        if (objectValue.ActiveFlag) MarkActive(objectValue.ActionedBy);
        else MarkInactive(objectValue.ActionedBy);

        return Result.Success();
    }

    private static Result Validate(decimal amount)
    {
        if (amount < 0)
        {
            return Result.Failure<TransactionDetail>(Errors.Transaction.AmountMustGreaterThanZero);
        }

        return Result.Success();
    }
}

