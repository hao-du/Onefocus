using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.SearchIndexQueue;

public record GetSearchIndexQueuesResponseDto(IReadOnlyList<Entity.SearchIndexQueue> Queues);
