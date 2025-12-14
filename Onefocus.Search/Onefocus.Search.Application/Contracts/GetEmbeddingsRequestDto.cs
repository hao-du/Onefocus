namespace Onefocus.Search.Application.Contracts;

public record GetEmbeddingsRequestDto(IReadOnlyList<string> Texts, bool Normalize = true);
