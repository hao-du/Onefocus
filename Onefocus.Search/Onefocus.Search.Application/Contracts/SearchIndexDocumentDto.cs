namespace Onefocus.Search.Application.Contracts;

public record SearchIndexDocumentDto(string? IndexName, string? EntityId, object Payload, Dictionary<string, string> VectorSearchTerms);
