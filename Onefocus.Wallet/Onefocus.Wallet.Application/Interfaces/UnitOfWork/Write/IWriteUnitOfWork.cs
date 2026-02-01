using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;

namespace Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;

public interface IWriteUnitOfWork
{
    IUserWriteRepository User { get; }
    IBankWriteRepository Bank { get; }
    ICurrencyWriteRepository Currency { get; }
    ICounterpartyWriteRepository Counterparty { get; }
    ITransactionWriteRepository Transaction { get; }
    ISearchIndexQueueWriteRepository SearchIndexQueue { get; }

    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default);
    Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default);
}