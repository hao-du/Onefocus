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
using Onefocus.Wallet.Domain.Events.Transaction;
using System.Threading;
using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Services
{
    internal class BankService(
        IWriteUnitOfWork unitOfWork, 
        ISearchIndexPublisher searchIndexPublisher
    ) : IBankService
    {
        public async Task<Result> HasDuplicatedBank(Guid id, string name, CancellationToken cancellationToken)
        {
            var spec = FindNameSpecification<Entity.Bank>.Create(name).And(ExcludeIdsSpecification<Entity.Bank>.Create([id]));
            var queryResult = await unitOfWork.Bank.GetBySpecificationAsync<Entity.Bank>(new(spec), cancellationToken);
            if (queryResult.IsFailure) return queryResult;
            if (queryResult.Value.Entity is not null) return Result.Failure(Errors.Bank.NameIsExisted);

            return Result.Success();
        }

        public async Task PublishEvents(Entity.Bank bank, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var entities = new List<ISearchIndexEntity>();
            foreach (var domainEvent in bank.DomainEvents)
            {
                if (domainEvent.EventType == typeof(BankUpsertedEvent).Name)
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
