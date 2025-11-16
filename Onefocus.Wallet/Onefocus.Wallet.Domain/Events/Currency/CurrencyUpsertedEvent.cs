using Onefocus.Common.Abstractions.Domain;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Currency;

public class CurrencyUpsertedEvent : IDomainEvent<WriteEntity.Currency>
{
    public WriteEntity.Currency Entity { get; private set; }
    public string IndexName => nameof(WriteEntity.Currency);
    public string EntityId => Entity.Id.ToString();
    public string Payload { get; private set; } = default!;
    public string EventType => GetType().Name;

    private CurrencyUpsertedEvent(WriteEntity.Currency currency)
    {
        Entity = currency;
        Payload = JsonSerializer.Serialize(new
        {
            name = currency.Name,
            ownerUserId = currency.OwnerUserId,
            shortName = currency.ShortName,
            description = currency.Description,
            isActive = currency.IsActive
        });
    }

    public static CurrencyUpsertedEvent Create(WriteEntity.Currency currency)
    {
        return new(currency);
    }
}