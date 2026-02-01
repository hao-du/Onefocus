using Onefocus.Common.Results;

namespace Onefocus.Wallet.Application.Interfaces.Services.Search;

public interface ISearchIndexManagementService
{
    Task<Result> ExecuteSearchIndexAsync(CancellationToken cancellationToken = default);
}