
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Constants;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class DomainEventService(
        IWriteUnitOfWork unitOfWork
    ) : IDomainEventService
    {
        public async Task<Result> AddSearchIndexEvent(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            var entities = domainEvents.Where(d => d.EventType == DomainEventTypes.SearchIndex).Select(d => Entity.SearchIndexQueue.Create(
                indexName: d.ObjectName,
                documentId: d.ObjectId,
                payload: d.Payload,
                vectorSearchTerms: d.KeyValuePairs
            ).Value).ToList();

            var result = await unitOfWork.SearchIndexQueue.AddSearchIndexQueueAsync(new(entities), cancellationToken);

            return result;
        }
    }
}
