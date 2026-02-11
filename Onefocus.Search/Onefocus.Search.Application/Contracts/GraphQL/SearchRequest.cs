namespace Onefocus.Search.Application.Contracts.GraphQL;

public class SearchRequest
{
    public string IndexName { get; set; } = default!;
    public Filter Filter { get; set; } = new();
    public Pagination? Paging { get; set; } = new();
    public List<Sorting>? Sort { get; set; } = [];
}
