using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Onefocus.Wallet.Domain.Entities.Write;

public class TransactionDetail: WriteEntityBase
{
    public decimal Amount { get; protected set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Enums.Action Action { get; protected set; }

    private TransactionDetail(decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, Guid actionedBy)
    {
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;

        Init(Guid.NewGuid(), description, actionedBy);
    }

    internal static Result<TransactionDetail> Create(decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, Guid actionedBy)
    {
        var validationResult = Validate(amount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransactionDetail>(validationResult.Error);
        }

        return new TransactionDetail(amount, transactedOn, action, description, actionedBy);
    }

    internal static Result<TransactionDetail> Create(ObjectValues.TransactionDetail objectValue)
    {
        var validationResult = Validate(objectValue.Amount);
        if (validationResult.IsFailure)
        {
            return Result.Failure<TransactionDetail>(validationResult.Error);
        }

        return new TransactionDetail(objectValue.Amount, objectValue.TransactedOn, objectValue.Action, objectValue.Description, objectValue.ActionedBy);
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

