using Onefocus.Common.Abstractions.ServiceBus.Search;

namespace Onefocus.Wallet.Application.Contracts.ServiceBus.Search;

public record SearchIndexDocument(string? IndexName, string? DocumentId, object Payload) : ISearchIndexDocument;