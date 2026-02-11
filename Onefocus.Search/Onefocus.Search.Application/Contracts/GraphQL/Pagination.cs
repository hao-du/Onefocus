namespace Onefocus.Search.Application.Contracts.GraphQL;

public class Pagination
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 20;
}
