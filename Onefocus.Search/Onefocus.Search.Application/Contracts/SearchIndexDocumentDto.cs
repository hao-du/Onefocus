namespace Onefocus.Search.Application.Contracts;

public record SearchIndexDocumentDto(string IndexName, string DocumentId, object Payload, Dictionary<string, string> VectorSearchTerms);
