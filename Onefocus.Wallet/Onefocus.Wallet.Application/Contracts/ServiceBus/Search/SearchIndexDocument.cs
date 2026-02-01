using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Wallet.Application.Contracts.ServiceBus.Search;

public record SearchIndexDocument(string IndexName, string DocumentId, string Payload, Dictionary<string, string> VectorSearchTerms) : ISearchIndexDocument;