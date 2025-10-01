using Onefocus.Home.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Home.Infrastructure.UnitOfWork.Read;

public class ReadUnitOfWork(HomeReadDbContext context
        , IUserReadRepository userRepository
        , IBankReadRepository bankRepository
        , ICurrencyReadRepository currencyRepository
        , ICounterpartyReadRepository counterpartyReadRepository
        , ITransactionReadRepository transactionRepository
    ) : IReadUnitOfWork
{
    private readonly WalletReadDbContext _context = context;
    public IUserReadRepository User { get; } = userRepository;
    public IBankReadRepository Bank { get; } = bankRepository;
    public ICurrencyReadRepository Currency { get; } = currencyRepository;
    public ICounterpartyReadRepository Counterparty { get; } = counterpartyReadRepository;
    public ITransactionReadRepository Transaction { get; } = transactionRepository;
}
