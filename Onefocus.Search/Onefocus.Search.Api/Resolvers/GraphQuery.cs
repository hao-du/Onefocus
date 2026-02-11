using HotChocolate.Authorization;
using Onefocus.Search.Application.Contracts.GraphQL;
using Onefocus.Search.Application.Interfaces.Services;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Api.Resolvers;

public class GraphQuery
{
    [Authorize]
    public async Task<Results.Result<string>> Search(SearchRequest request, [Service] ISearchQueryService searchService)
    {
        var result = await searchService.SearchAsync(request);
        return result;
    }
}
