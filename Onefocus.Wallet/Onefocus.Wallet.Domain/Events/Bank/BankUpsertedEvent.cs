using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using System.Text.Json;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Bank;

public class BankUpsertedEvent: IDomainEvent<WriteEntity.Bank>
{
    public WriteEntity.Bank Entity { get; private set; }
    public string IndexName => SchemaNames.Bank;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;

    private BankUpsertedEvent(WriteEntity.Bank bank)
    {
        Entity = bank;
        Payload = JsonSerializer.Serialize(new
        {
            name = bank.Name,
            ownerUserId = bank.OwnerUserId,
            description = bank.Description,
            isActive = bank.IsActive
        });
    }

    public static BankUpsertedEvent Create(WriteEntity.Bank bank)
    {
        return new(bank);
    }
}