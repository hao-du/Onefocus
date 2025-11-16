using Onefocus.Common.Abstractions.Domain;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Bank;

public class BankCreatedEvent: IDomainEvent<WriteEntity.Bank>
{
    public WriteEntity.Bank Entity { get; private set; }
    public string EntityName => nameof(WriteEntity.Bank);
    public string EntityId => Entity.Id.ToString();
    public string Payload { get; private set; } = default!;
    public string EventType => typeof(BankCreatedEvent).Name;

    private BankCreatedEvent(WriteEntity.Bank bank)
    {
        Entity = bank;
        Payload = JsonSerializer.Serialize(new
        {
            name = bank.Name,
            description = bank.Description,
            isActive = bank.IsActive
        });
    }

    public static BankCreatedEvent Create(WriteEntity.Bank bank)
    {
        return new(bank);
    }
}