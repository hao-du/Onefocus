using Onefocus.Common.Results;

namespace Onefocus.Search.Application.Interfaces.Services;

public interface ISearchIndexManagementService
{
    Task<Result> ExecuteSearchIndexAsync(CancellationToken cancellationToken = default);
}