namespace Onefocus.Search.Application.Contracts;

public record GetEmbeddingsResponseItemDto(string Text, List<float> Embeddings);
