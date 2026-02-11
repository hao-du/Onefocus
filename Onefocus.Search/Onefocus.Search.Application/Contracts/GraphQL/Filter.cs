using Onefocus.Search.Application.Contracts.GraphQL.Filters;

namespace Onefocus.Search.Application.Contracts.GraphQL;

public class Filter
{
    public List<TermFilter>? TermFilters { get; set; } = [];
    public List<RangeFilter>? RangeFilters { get; set; } = [];
    public List<MatchFilter>? MatchFilters { get; set; } = [];
    public List<SemanticFilter>? SemanticFilters { get; set; } = [];
}
