using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class CashFlowUpsertedEvent : IDomainEvent<WriteEntity.CashFlow>
{
    public WriteEntity.CashFlow Entity { get; private set; }
    public string IndexName => SchemaNames.Transaction;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;

    private CashFlowUpsertedEvent(WriteEntity.CashFlow cashFlow)
    {
        Entity = cashFlow;
        Payload = new
        {
            id = cashFlow.Id,
            type = nameof(WriteEntity.CashFlow),
            currencyId = cashFlow.Transaction.CurrencyId,
            currencyName = cashFlow.Transaction.Currency.Name,
            transactedOn = cashFlow.Transaction.TransactedOn,
            ownerUserId = cashFlow.Transaction.OwnerUserId,
            description = cashFlow.Transaction.Description,
            isActive = cashFlow.Transaction.IsActive,
            items = cashFlow.Transaction.TransactionItems.Select(item => new
            {
                id = item.Id,
                name = item.Name,
                description = item.Description,
                isActive = cashFlow.Transaction.IsActive,
            }).ToArray()
        };
    }

    public static CashFlowUpsertedEvent Create(WriteEntity.CashFlow cashFlow)
    {
        return new(cashFlow);
    }
}