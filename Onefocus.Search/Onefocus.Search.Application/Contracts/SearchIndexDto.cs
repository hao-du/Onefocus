using System.Text.Json;

namespace Onefocus.Search.Application.Contracts;

public record SearchIndexDto(string? EntityType, string? EntityId, JsonElement Payload);
