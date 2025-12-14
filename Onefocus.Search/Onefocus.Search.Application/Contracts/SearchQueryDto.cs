namespace Onefocus.Search.Application.Contracts;

public record SearchQueryDto(string IndexName, object Query, Dictionary<string, string> VectorSearchTerms);
