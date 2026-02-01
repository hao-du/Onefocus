using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.SearchIndexQueue;

namespace Onefocus.Wallet.Application.Interfaces.Repositories.Write;

public interface ISearchIndexQueueWriteRepository : IBaseContextRepository
{
    Task<Result> AddSearchIndexQueueAsync(AddSearchIndexQueueRequestDto request, CancellationToken cancellationToken = default);
}