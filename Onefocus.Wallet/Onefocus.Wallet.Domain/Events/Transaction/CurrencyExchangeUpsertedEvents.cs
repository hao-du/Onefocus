using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class CurrencyExchangeUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.CurrencyExchange currencyExchange)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: currencyExchange.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = currencyExchange.Id,
                type = nameof(WriteEntity.CurrencyExchange),
                description = currencyExchange.Description,
                isActive = currencyExchange.IsActive,
                transactions = currencyExchange.CurrencyExchangeTransactions.Select(cet => new
                {
                    id = cet.Transaction.Id,
                    transactedOn = cet.Transaction.TransactedOn,
                    currencyId = cet.Transaction.CurrencyId,
                    isTarget = cet.IsTarget,
                    description = cet.Transaction.Description,
                    isActive = cet.Transaction.IsActive,
                }).ToArray()
            }),
            keyValuePairs: []
        );
    }
}