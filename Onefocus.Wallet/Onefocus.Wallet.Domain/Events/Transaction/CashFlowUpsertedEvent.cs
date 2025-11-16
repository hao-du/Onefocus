using Onefocus.Common.Abstractions.Domain;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class CashFlowUpsertedEvent : IDomainEvent<WriteEntity.CashFlow>
{
    public WriteEntity.CashFlow Entity { get; private set; }
    public string IndexName => nameof(Entities.Write.Transaction);
    public string EntityId => Entity.Id.ToString();
    public string Payload { get; private set; } = default!;
    public string EventType => GetType().Name;

    private CashFlowUpsertedEvent(WriteEntity.CashFlow cashFlow)
    {
        Entity = cashFlow;
        Payload = JsonSerializer.Serialize(new
        {
            type = nameof(WriteEntity.CashFlow),
            currencyId = cashFlow.Transaction.CurrencyId,
            currencyName = cashFlow.Transaction.Currency.Name,
            transactedOn = cashFlow.Transaction.TransactedOn,
            ownerUserId = cashFlow.Transaction.OwnerUserId,
            description = cashFlow.Transaction.Description,
            isActive = cashFlow.Transaction.IsActive,
            transactions = cashFlow.Transaction.TransactionItems.Select(item => new
            {
                name = item.Name,
                description = item.Description,
                isActive = cashFlow.Transaction.IsActive,
            }).ToArray()
        });
    }

    public static CashFlowUpsertedEvent Create(WriteEntity.CashFlow cashFlow)
    {
        return new(cashFlow);
    }
}