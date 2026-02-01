using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.SearchIndexQueue;

public sealed record AddSearchIndexQueueRequestDto(IReadOnlyList<Entity.SearchIndexQueue> domainEvents);