using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Models;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class TransactionDetail: WriteEntityBase
{
    public Guid TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTimeOffset TransactedOn { get; private set; }
    public Enums.Action Action { get; private set; }
    public Guid ActionedBy { get; private set; }
    public Transaction Transaction { get; private set; } = default!;

    private TransactionDetail(Guid transactionId, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string? description, Guid actionedBy)
    {
        TransactionId = transactionId;
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;

        Init(Guid.NewGuid(), description, actionedBy);
    }

    internal static Result<TransactionDetail> Create(TransactionDetailParams detailParams)
    {
        var validationResult = Validate(detailParams.Amount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransactionDetail>(validationResult.Error);
        }

        return new TransactionDetail(detailParams.TransactionId, detailParams.Amount, detailParams.TransactedOn, detailParams.Action, detailParams.Description, detailParams.ActionedBy);
    }

    internal Result Update(TransactionDetailParams detailParams)
    {
        var validationResult = Validate(detailParams.Amount);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Amount = detailParams.Amount;
        TransactedOn = detailParams.TransactedOn;
        Action = detailParams.Action;
        Description = detailParams.Description;

        if (detailParams.ActiveFlag) MarkActive(detailParams.ActionedBy);
        else MarkInactive(detailParams.ActionedBy);

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

