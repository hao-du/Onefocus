using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class OutboxEvent : ReadEntityBase
{
    public string IndexName { get; init; } = default!;
    public string DocumentId { get; init; } = default!;
    public string Payload { get; init; } = default!;
    public Dictionary<string, string> VectorSearchTerms { get; init; } = [];
}
