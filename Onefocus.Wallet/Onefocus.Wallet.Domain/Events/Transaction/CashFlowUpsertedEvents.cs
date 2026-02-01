using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class CashFlowUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.CashFlow cashFlow)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: cashFlow.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = cashFlow.Id,
                type = nameof(WriteEntity.CashFlow),
                currencyId = cashFlow.Transaction.CurrencyId,
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
            }),
            keyValuePairs: []
        );
    }
}