using System.Text.Json;

namespace Onefocus.Search.Application.Contracts;

public record SearchIndexDto(string? IndexName, string? EntityId, JsonElement Payload);
