using Onefocus.Common.UnitOfWork;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;

namespace Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;

public interface IWriteUnitOfWork : IBaseUnitOfWork
{
    IUserWriteRepository User { get; }
    IBankWriteRepository Bank { get; }
    ICurrencyWriteRepository Currency { get; }
    ICounterpartyWriteRepository Counterparty { get; }
    ITransactionWriteRepository Transaction { get; }
    ISearchIndexQueueWriteRepository SearchIndexQueue { get; }
}