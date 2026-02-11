using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts.GraphQL;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface ISearchQueryService
{
    Task<Result<string>> SearchAsync(SearchRequest searchQueryDto, CancellationToken cancellationToken = default);
}
