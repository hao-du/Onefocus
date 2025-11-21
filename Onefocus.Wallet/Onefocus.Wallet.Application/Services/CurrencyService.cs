using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Events.Currency;
using Onefocus.Wallet.Domain.Specifications.Write.Currency;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class CurrencyService(
        IWriteUnitOfWork unitOfWork,
        ISearchIndexPublisher searchIndexPublisher
    ) : ICurrencyService
    {
        public async Task<Result> HasDuplicatedCurrency(Guid id, string name, string shortName, CancellationToken cancellationToken)
        {
            var orSpec = new OrSpecification<Entity.Currency>(
                FindNameSpecification<Entity.Currency>.Create(name),
                FindShortNameSpecification.Create(shortName)
            );
            var spec = orSpec.And(ExcludeIdsSpecification<Entity.Currency>.Create([id]));

            var queryResult = await unitOfWork.Currency.GetBySpecificationAsync<Entity.Currency>(new(spec), cancellationToken);
            if (queryResult.IsFailure) return queryResult;
            if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Currency.NameOrShortNameIsExisted);

            return Result.Success();
        }

        public async Task PublishEvents(Entity.Currency currency, CancellationToken cancellationToken)
        {
            var documents = new List<ISearchIndexDocument>();
            foreach (var domainEvent in currency.DomainEvents)
            {
                if (domainEvent.EventType == typeof(CurrencyUpsertedEvent).Name)
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
