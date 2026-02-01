using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.Params;
using Onefocus.Wallet.Domain.Events.Transaction;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CashFlow : WriteEntityBase, IAggregateRoot
{
    public Guid TransactionId { get; private set; }
    public bool IsIncome { get; private set; }

    public Entity.Transaction Transaction { get; private set; } = default!;

    private CashFlow()
    {
        // Required for EF Core
    }

    private CashFlow(bool isIncome, string? description, Guid actionedBy)
    {
        Init(description, actionedBy);

        IsIncome = isIncome;
    }

    public static Result<CashFlow> Create(decimal amount, DateTimeOffset transactedOn, bool isIncome, Currency currency, string? description, Guid ownerId, Guid actionedBy, IReadOnlyList<TransactionItemParams> transactionItems)
    {
        var cashFlow = new CashFlow(isIncome, description, actionedBy);

        var createTransactionResult = cashFlow.CreateTransaction(ownerId, actionedBy, TransactionParams.Create(
                id: null,
                amount: amount,
                transactedOn: transactedOn,
                currency: currency,
                description: description,
                isActive: true,
                transactionItems: transactionItems
            ));
        if (createTransactionResult.IsFailure) return (Result<CashFlow>)createTransactionResult;

        cashFlow.AddDomainEvent(CashFlowUpsertedEvents.AddSearchIndex(cashFlow));

        return Result.Success(cashFlow);
    }

    public Result Update(decimal amount, DateTimeOffset transactedOn, bool isIncome, Currency currency, string? description, Guid actionedBy, bool isActive, IReadOnlyList<TransactionItemParams> transactionItems)
    {
        IsIncome = isIncome;
        Description = description;

        SetActiveFlag(isActive, actionedBy);

        var updateTransactionResult = UpdateTransaction(actionedBy, TransactionParams.Create(
                id: null,
                amount: amount,
                transactedOn: transactedOn,
                currency: currency,
                description: description,
                isActive: isActive,
                transactionItems: transactionItems
            ));
        if (updateTransactionResult.IsFailure) return updateTransactionResult;

        AddDomainEvent(CashFlowUpsertedEvents.AddSearchIndex(this));

        return Result.Success();
    }

    private Result CreateTransaction(Guid ownerId, Guid actionedBy, TransactionParams param)
    {
        if (Transaction != null)
        {
            return Result.Failure(Errors.Transaction.TransactionExists);
        }

        var createTransactionResult = Entity.Transaction.Create(
                    amount: param.Amount,
                    transactedOn: param.TransactedOn,
                    currency: param.Currency,
                    description: param.Description,
                    ownerId: ownerId,
                    actionedBy: actionedBy,
                    transactionItems: param.TransactionItems
                );
        if (createTransactionResult.IsFailure) return createTransactionResult;
        Transaction = createTransactionResult.Value;

        return Result.Success();
    }

    private Result UpdateTransaction(Guid actionedBy, TransactionParams param)
    {
        if (Transaction == null)
        {
            return Result.Failure(Errors.Transaction.InvalidTransaction);
        }

        Transaction.Update(
                amount: param.Amount,
                transactedOn: param.TransactedOn,
                currency: param.Currency,
                description: param.Description,
                isActive: param.IsActive,
                actionedBy: actionedBy,
                transactionItems: param.TransactionItems
            );

        return Result.Success();
    }
}

