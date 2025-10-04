using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Write;

namespace Onefocus.Home.Application.Interfaces.UnitOfWork.Write;

public interface IWriteUnitOfWork
{
    IUserWriteRepository User { get; }
    ISettingsWriteRepository Settings { get; }

    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default);
    Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default);
}