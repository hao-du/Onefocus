using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Domain.Events.Counterparty;
using Onefocus.Wallet.Domain.Events.Currency;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class CounterpartyService(
        ISearchIndexPublisher searchIndexPublisher
    ) : ICounterpartyService
    {
        public async Task PublishEvents(Entity.Counterparty counterparty, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var entities = new List<ISearchIndexEntity>();
            foreach (var domainEvent in counterparty.DomainEvents)
            {
                if (domainEvent.EventType == typeof(CounterpartyUpsertedEvent).Name)
                {
                    entities.Add(new SearchIndexEntity(
                        IndexName: domainEvent.IndexName,
                        EntityId: domainEvent.EntityId,
                        Payload: domainEvent.Payload)
                    );
                }
            }
            if (entities.Count > 0)
            {
                await searchIndexPublisher.Publish(new SearchIndexMessage(entities), cancellationToken);
            }
        }
    }
}
