using Onefocus.Common.Results;

namespace Onefocus.Common.UnitOfWork;

public interface IBaseUnitOfWork
{
    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default);
    Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default);
}

