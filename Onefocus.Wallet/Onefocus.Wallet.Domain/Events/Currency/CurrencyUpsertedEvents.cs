using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Currency;

public class CurrencyUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.Currency currency)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: currency.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                name = currency.Name,
                ownerUserId = currency.OwnerUserId,
                shortName = currency.ShortName,
                description = currency.Description,
                isActive = currency.IsActive
            }),
            keyValuePairs: []
        );
    }
}