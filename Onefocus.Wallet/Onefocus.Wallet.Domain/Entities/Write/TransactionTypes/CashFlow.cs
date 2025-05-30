using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Write.Params;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CashFlow : BaseTransaction, IAggregateRoot
{
    public CashFlowDirection Direction { get; private set; }

    private CashFlow() : base()
    {
    }

    public CashFlow(CashFlowDirection direction, string? description, Guid actionedBy)
    {
        Init(Guid.NewGuid(), description, actionedBy);

        Direction = direction;
    }

    public static Result<CashFlow> Create(CashFlowDirection direction, string? description, Guid actionedBy)
    {
        return new CashFlow(direction, description, actionedBy);
    }

    protected override Result CreateTransaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        if (_transactions.Count > 0)
        {
            return Result.Failure(Errors.Transaction.TransactionExists);
        }

        return base.CreateTransaction(amount, transactedOn, currencyId, description, actionedBy, transactionItems);
    }

    public Result CreateCashFlowTransaction(decimal amount, DateTimeOffset transactedOn, Guid currencyId, string? description, Guid actionedBy, IReadOnlyList<TransactionItemParams>? transactionItems = null)
    {
        return CreateTransaction(amount, transactedOn, currencyId, description, actionedBy, transactionItems);
    }

    public Result Update(CashFlowDirection direction, string? description, bool isActive, Guid actionedBy, TransactionParams? transaction)
    {
        Direction = direction;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return UpsertTransaction(transaction, actionedBy);
    }

    public Result Update(CashFlowDirection direction, bool isActive, string? description, Guid actionedBy)
    {
        Direction = direction;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return Result.Success();
    }
}

