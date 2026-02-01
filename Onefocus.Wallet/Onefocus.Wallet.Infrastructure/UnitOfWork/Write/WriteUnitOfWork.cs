using Microsoft.Extensions.Logging;
using Onefocus.Common.UnitOfWork;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

public class WriteUnitOfWork(WalletWriteDbContext context
        , ILogger<WriteUnitOfWork> logger
        , IUserWriteRepository userRepository
        , IBankWriteRepository bankRepository
        , ICurrencyWriteRepository currencyRepository
        , ICounterpartyWriteRepository counterpartyWriteRepository
        , ITransactionWriteRepository transactionRepository
        , ISearchIndexQueueWriteRepository searchIndexQueueRepository
    ) : BaseUnitOfWork(context, logger), IWriteUnitOfWork
{
    protected ILogger<WriteUnitOfWork> Logger { get; } = logger;
    public IUserWriteRepository User { get; } = userRepository;
    public IBankWriteRepository Bank { get; } = bankRepository;
    public ICurrencyWriteRepository Currency { get; } = currencyRepository;
    public ICounterpartyWriteRepository Counterparty { get; } = counterpartyWriteRepository;
    public ITransactionWriteRepository Transaction { get; } = transactionRepository;
    public ISearchIndexQueueWriteRepository SearchIndexQueue { get; } = searchIndexQueueRepository;
}
