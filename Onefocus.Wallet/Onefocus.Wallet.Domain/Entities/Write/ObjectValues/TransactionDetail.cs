using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.ObjectValues;

public class TransactionDetail
{
    public Guid TransactionId { get; protected set; }
    public Guid Id { get; protected set; } = default;
    public decimal Amount { get; protected set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Enums.Action Action { get; protected set; }
    public string Description { get; protected set; }
    public bool ActiveFlag { get; protected set; }
    public Guid ActionedBy { get; protected set; }

    private TransactionDetail(Guid transactionId, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        TransactionId = transactionId;
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;
        Description = description;
        ActionedBy = actionedBy;
    }

    private TransactionDetail(Guid id, Guid transactionId, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        Id = id;
        TransactionId = transactionId;
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;
        Description = description;
        ActionedBy = actionedBy;
    }

    public static Result<TransactionDetail> Create(Guid transactionId, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransactionDetail>(Errors.Transaction.AmountMustGreaterThanZero);
        }

        return new TransactionDetail(transactionId, amount, transactedOn, action, description, activeFlag, actionedBy);
    }

    public static Result<TransactionDetail> Create(Guid transactionId, Guid id, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransactionDetail>(Errors.Transaction.AmountMustGreaterThanZero);
        }

        return new TransactionDetail(id, transactionId, amount, transactedOn, action, description, activeFlag, actionedBy);
    }
}

