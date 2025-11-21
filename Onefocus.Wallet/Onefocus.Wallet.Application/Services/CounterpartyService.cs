using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Domain.Events.Counterparty;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class CounterpartyService(
        ISearchIndexPublisher searchIndexPublisher
    ) : ICounterpartyService
    {
        public async Task PublishEvents(Entity.Counterparty counterparty, CancellationToken cancellationToken)
        {
            var documents = new List<ISearchIndexDocument>();
            foreach (var domainEvent in counterparty.DomainEvents)
            {
                if (domainEvent.EventType == typeof(CounterpartyUpsertedEvent).Name)
                {
                    documents.Add(new SearchIndexDocument(
                        IndexName: domainEvent.IndexName,
                        DocumentId: domainEvent.EntityId,
                        Payload: domainEvent.Payload)
                    );
                }
            }
            if (documents.Count > 0)
            {
                await searchIndexPublisher.Publish(new SearchIndexMessage(documents), cancellationToken);
            }
        }
    }
}
