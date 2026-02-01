using Entity = Onefocus.Search.Domain.Entities;

namespace Onefocus.Search.Application.Contracts.SearchIndexQueue;

public record GetSearchIndexQueuesResponseDto(IReadOnlyList<Entity.SearchIndexQueue> Queues);
