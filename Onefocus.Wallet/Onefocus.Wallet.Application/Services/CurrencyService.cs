using Onefocus.Common.Abstractions.Domain.Specification;
using Onefocus.Common.Abstractions.Domain.Specifications;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Membership.Application.Contracts.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Events.Bank;
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
            var tasks = new List<Task>();
            var entities = new List<ISearchIndexEntity>();
            foreach (var domainEvent in currency.DomainEvents)
            {
                if (domainEvent.EventType == typeof(CurrencyUpsertedEvent).Name)
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
