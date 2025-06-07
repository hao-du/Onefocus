using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Repositories.Write;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

public interface IWriteUnitOfWork
{
    IUserWriteRepository User { get; }
    IBankWriteRepository Bank { get; }
    ICurrencyWriteRepository Currency { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default);
    Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default);
}