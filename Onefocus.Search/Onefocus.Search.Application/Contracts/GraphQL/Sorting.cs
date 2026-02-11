namespace Onefocus.Search.Application.Contracts.GraphQL;

public class Sorting
{
    public string Field { get; set; } = default!;
    public SortDirection Order { get; set; } = SortDirection.ASC;
}
