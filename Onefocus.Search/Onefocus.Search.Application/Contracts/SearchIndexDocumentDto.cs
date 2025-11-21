using System.Text.Json;

namespace Onefocus.Search.Application.Contracts;

public record SearchIndexDocumentDto(string? IndexName, string? EntityId, object Payload);
