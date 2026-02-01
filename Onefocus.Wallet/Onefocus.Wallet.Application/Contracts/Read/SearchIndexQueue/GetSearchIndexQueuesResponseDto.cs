using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.SearchIndexQueue;

public record GetSearchIndexQueuesResponseDto(IReadOnlyList<Entity.SearchIndexQueue> Queues);
