using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Bank;

public class BankUpsertedEvents
{
    public static IDomainEvent AddSearchIndex(WriteEntity.Bank bank)
    {
        return DomainEvent.Create(
            eventType: DomainEventTypes.SearchIndex,
            objectName: SchemaNames.Bank,
            objectId: bank.Id.ToString(),
            payload: JsonHelper.SerializeJson(new
            {
                id = bank.Id,
                name = bank.Name,
                ownerUserId = bank.OwnerUserId,
                description = bank.Description,
                isActive = bank.IsActive,
                embedding = "{{knn_vectorsearch_bank}}"
            }),
            keyValuePairs: new Dictionary<string, string>()
            {
                { "{{knn_vectorsearch_bank}}", $"{bank.Name} {bank.Description}" }
            }
        );
    }
}