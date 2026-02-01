using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.SearchIndexQueue;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface ISearchIndexQueueReadRepository : IBaseContextRepository
{
    Task<Result<GetSearchIndexQueuesResponseDto>> GetSearchIndexQueuesAsync(GetSearchIndexQueuesRequestDto request, CancellationToken cancellationToken = default);
}