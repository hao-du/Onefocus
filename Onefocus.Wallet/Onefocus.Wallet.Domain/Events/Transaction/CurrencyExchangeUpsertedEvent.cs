using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Constants;
using WriteEntity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Domain.Events.Transaction;

public class CurrencyExchangeUpsertedEvent : IDomainEvent<WriteEntity.CurrencyExchange>
{
    public WriteEntity.CurrencyExchange Entity { get; private set; }
    public string IndexName => SchemaNames.Transaction;
    public string EntityId => Entity.Id.ToString();
    public object Payload { get; private set; }
    public string EventType => GetType().Name;
    public Dictionary<string, string> VectorSearchTerms { get; } = [];

    private CurrencyExchangeUpsertedEvent(WriteEntity.CurrencyExchange currencyExchange)
    {
        Entity = currencyExchange;
        Payload = new
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
        };
    }

    public static CurrencyExchangeUpsertedEvent Create(WriteEntity.CurrencyExchange currencyExchange)
    {
        return new(currencyExchange);
    }
}