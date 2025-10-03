using Onefocus.Home.Application.Interfaces.Repositories.Read;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Read;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Home.Infrastructure.UnitOfWork.Read;

public class ReadUnitOfWork(HomeReadDbContext context
        , ISettingReadRepository settingRepository
    ) : IReadUnitOfWork
{
    private readonly HomeReadDbContext _context = context;
    public ISettingReadRepository Setting { get; } = settingRepository;
}
