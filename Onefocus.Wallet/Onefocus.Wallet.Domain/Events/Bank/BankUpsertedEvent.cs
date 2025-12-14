using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Events.Bank;

public class BankUpsertedEvent : IDomainEvent<WriteEntity.Bank>
{
    public WriteEntity.Bank Entity { get; private set; }
    public string IndexName => SchemaNames.Bank;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;
    public Dictionary<string, string> VectorSearchTerms { get; private set; }

    private BankUpsertedEvent(WriteEntity.Bank bank)
    {
        Entity = bank;
        Payload = new
        {
            id = bank.Id,
            name = bank.Name,
            ownerUserId = bank.OwnerUserId,
            description = bank.Description,
            isActive = bank.IsActive,
            embedding = "{{knn_vectorsearch_bank}}"
        };
        VectorSearchTerms = new Dictionary<string, string>()
        {
            { "{{knn_vectorsearch_bank}}", $"{bank.Name} {bank.Description}" }
        };
    }

    public static BankUpsertedEvent Create(WriteEntity.Bank bank)
    {
        return new(bank);
    }
}