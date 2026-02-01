using Entity = Onefocus.Search.Domain.Entities;

namespace Onefocus.Search.Application.Contracts.SearchIndexQueue;

public sealed record AddSearchIndexQueueRequestDto(IReadOnlyList<Entity.SearchIndexQueue> searchIndexQueues);