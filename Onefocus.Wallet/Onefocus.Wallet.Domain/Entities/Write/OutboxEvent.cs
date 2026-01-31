using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class OutboxEvent : WriteEntityBase
{
    public string IndexName { get; private set; } = default!;
    public string DocumentId { get; private set; } = default!;
    public string Payload { get; private set; } = default!;
    public Dictionary<string, string> VectorSearchTerms { get; private set; } = [];
}
