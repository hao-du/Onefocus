using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Search.Domain.Entities;

public sealed class SearchIndexQueue : WriteEntityBase
{
    public string IndexName { get; private set; } = default!;
    public string DocumentId { get; private set; } = default!;
    public string Payload { get; private set; } = default!;
    public Dictionary<string, string> VectorSearchTerms { get; private set; } = [];

    private SearchIndexQueue()
    {
        // Required for EF Core
    }

    private SearchIndexQueue(string indexName, string documentId, string payload, Dictionary<string, string> vectorSearchTerms)
    {
        Init(default, Guid.Empty);

        IndexName = indexName;
        DocumentId = documentId;
        Payload = payload;
        VectorSearchTerms = vectorSearchTerms;
    }

    public static Result<SearchIndexQueue> Create(string indexName, string documentId, string payload, Dictionary<string, string> vectorSearchTerms)
    {
        var outboxEvent = new SearchIndexQueue(indexName, documentId, payload, vectorSearchTerms);

        return outboxEvent;
    }
}
