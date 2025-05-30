using Onefocus.Wallet.Domain.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

public class ReadUnitOfWork(WalletReadDbContext context
        , IUserReadRepository userRepository
        , ICurrencyReadRepository currencyRepository
    ) : IReadUnitOfWork
{
    private readonly WalletReadDbContext _context = context;
    public IUserReadRepository User { get; } = userRepository;
    public ICurrencyReadRepository Currency { get; } = currencyRepository;
}
