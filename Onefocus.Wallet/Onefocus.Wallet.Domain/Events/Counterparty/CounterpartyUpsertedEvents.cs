using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Counterparty;

public class CounterpartyUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.Counterparty counterparty)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Counterparty,
            objectId: counterparty.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = counterparty.Id,
                name = counterparty.FullName,
                phone = counterparty.PhoneNumber,
                email = counterparty.Email,
                ownerUserId = counterparty.OwnerUserId,
                description = counterparty.Description,
                isActive = counterparty.IsActive
            }),
            keyValuePairs: []
        );
    }
}