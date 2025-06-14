using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

public class ReadUnitOfWork(WalletReadDbContext context
        , IUserReadRepository userRepository
        , IBankReadRepository bankRepository
        , ICurrencyReadRepository currencyRepository
        , ITransactionReadRepository transactionRepository
    ) : IReadUnitOfWork
{
    private readonly WalletReadDbContext _context = context;
    public IUserReadRepository User { get; } = userRepository;
    public IBankReadRepository Bank { get; } = bankRepository;
    public ICurrencyReadRepository Currency { get; } = currencyRepository;
    public ITransactionReadRepository Transaction { get; } = transactionRepository;
}
