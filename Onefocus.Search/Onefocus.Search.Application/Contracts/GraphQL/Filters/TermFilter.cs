namespace Onefocus.Search.Application.Contracts.GraphQL.Filters;

public class TermFilter
{
    public string Field { get; set; } = default!;
    public object? Value { get; set; }
}
