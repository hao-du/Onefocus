namespace Onefocus.Search.Application.Contracts.GraphQL.Filters;

public class RangeFilter
{
    public string Field { get; set; } = default!;
    public object? Gte { get; set; }
    public object? Lte { get; set; }
}
