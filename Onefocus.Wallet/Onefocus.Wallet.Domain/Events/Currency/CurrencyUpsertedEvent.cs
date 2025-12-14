using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Currency;

public class CurrencyUpsertedEvent : IDomainEvent<WriteEntity.Currency>
{
    public WriteEntity.Currency Entity { get; private set; }
    public string IndexName => SchemaNames.Counterparty;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;
    public Dictionary<string, string> VectorSearchTerms { get; } = [];

    private CurrencyUpsertedEvent(WriteEntity.Currency currency)
    {
        Entity = currency;
        Payload = new
        {
            name = currency.Name,
            ownerUserId = currency.OwnerUserId,
            shortName = currency.ShortName,
            description = currency.Description,
            isActive = currency.IsActive
        };
    }

    public static CurrencyUpsertedEvent Create(WriteEntity.Currency currency)
    {
        return new(currency);
    }
}