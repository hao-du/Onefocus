using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Counterparty;

public class CounterpartyUpsertedEvent : IDomainEvent<WriteEntity.Counterparty>
{
    public WriteEntity.Counterparty Entity { get; private set; }
    public string IndexName => SchemaNames.Counterparty;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;

    private CounterpartyUpsertedEvent(WriteEntity.Counterparty counterparty)
    {
        Entity = counterparty;
        Payload = JsonSerializer.Serialize(new
        {
            name = counterparty.FullName,
            phone = counterparty.PhoneNumber,
            email = counterparty.Email,
            ownerUserId = counterparty.OwnerUserId,
            description = counterparty.Description,
            isActive = counterparty.IsActive
        });
    }

    public static CounterpartyUpsertedEvent Create(WriteEntity.Counterparty counterparty)
    {
        return new(counterparty);
    }
}