namespace Onefocus.Search.Application.Contracts.GraphQL.Filters;

public class SemanticFilter
{
    public string Field { get; set; } = default!;
    public string Value { get; set; } = default!;
}
