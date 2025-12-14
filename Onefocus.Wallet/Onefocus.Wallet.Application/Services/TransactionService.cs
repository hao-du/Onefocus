using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using Onefocus.Wallet.Domain.Events.Transaction;

namespace Onefocus.Wallet.Application.Services;

internal class TransactionService(
    IWriteUnitOfWork unitOfWork,
    ISearchIndexPublisher searchIndexPublisher
) : ITransactionService
{
    public Task PublishEvents(BankAccount bankAccount, CancellationToken cancellationToken)
    {
        return PublishEvents<BankAccount, BankAccountUpsertedEvent>(bankAccount, cancellationToken);
    }

    public Task PublishEvents(CashFlow cashFlow, CancellationToken cancellationToken)
    {
        return PublishEvents<CashFlow, CashFlowUpsertedEvent>(cashFlow, cancellationToken);
    }

    public Task PublishEvents(CurrencyExchange currencyExchange, CancellationToken cancellationToken)
    {
        return PublishEvents<CurrencyExchange, CurrencyExchangeUpsertedEvent>(currencyExchange, cancellationToken);
    }

    public Task PublishEvents(PeerTransfer peerTransfer, CancellationToken cancellationToken)
    {
        return PublishEvents<PeerTransfer, PeerTransferUpsertedEvent>(peerTransfer, cancellationToken);
    }

    private async Task PublishEvents<Entity, Event>(Entity entity, CancellationToken cancellationToken) where Entity : WriteEntityBase where Event : IDomainEvent<Entity>
    {
        var documents = new List<ISearchIndexDocument>();
        foreach (var domainEvent in entity.DomainEvents)
        {
            if (domainEvent.EventType == typeof(Event).Name)
            {
                documents.Add(new SearchIndexDocument(
                    IndexName: domainEvent.IndexName,
                    DocumentId: domainEvent.EntityId,
                    Payload: domainEvent.Payload,
                    VectorSearchTerms: domainEvent.VectorSearchTerms)
                );
            }
        }
        if (documents.Count > 0)
        {
            await searchIndexPublisher.Publish(new SearchIndexMessage(documents), cancellationToken);
        }
    }
}
