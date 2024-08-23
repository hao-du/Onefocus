using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Transactions;

namespace Onefocus.Wallet.Domain.Entities.ObjectValues;

public class TransactionDetail
{
    public Guid Id { get; protected set; } = default(Guid);
    public decimal Amount { get; protected set; }
    public DateTimeOffset TransactedOn { get; protected set; }
    public Enums.Action Action { get; protected set; }
    public string Description { get; protected set; }
    public bool ActiveFlag { get; protected set; }
    public Guid ActionedBy { get; protected set; }

    private TransactionDetail(decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;
        Description = description;
        ActionedBy = actionedBy;
    }

    private TransactionDetail(Guid id, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        Id = id;
        Amount = amount;
        TransactedOn = transactedOn;
        Action = action;
        Description = description;
        ActionedBy = actionedBy;
    }

    public static Result<TransactionDetail> Create(decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransactionDetail>(Errors.Transaction.AmountMustGreaterThanZero);
        }

        return new TransactionDetail(amount, transactedOn, action, description, activeFlag, actionedBy);
    }

    public static Result<TransactionDetail> Create(Guid id, decimal amount, DateTimeOffset transactedOn, Enums.Action action, string description, bool activeFlag, Guid actionedBy)
    {
        if (amount < 0)
        {
            return Result.Failure<TransactionDetail>(Errors.Transaction.AmountMustGreaterThanZero);
        }

        return new TransactionDetail(id, amount, transactedOn, action, description, activeFlag, actionedBy);
    }
}

