using Onefocus.Wallet.Domain.Repositories.Read;
using Onefocus.Wallet.Infrastructure.UnitOfWork.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

public class ReadUnitOfWork : IReadUnitOfWork
{
    private readonly WalletReadDbContext _context;
    public IUserReadRepository User { get; }
    public ICurrencyReadRepository Currency { get; }

    public ReadUnitOfWork(WalletReadDbContext context
        , IUserReadRepository userRepository
        , ICurrencyReadRepository currencyRepository
    )
    {
        _context = context;
        User = userRepository;
        Currency = currencyRepository;
    }
}
