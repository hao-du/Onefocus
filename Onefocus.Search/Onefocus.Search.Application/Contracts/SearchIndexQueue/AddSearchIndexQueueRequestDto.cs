namespace Onefocus.Search.Application.Contracts.SearchIndexQueue;

public sealed record BulkUpdateActiveStatusRequestDto(IReadOnlyList<Guid> Ids);