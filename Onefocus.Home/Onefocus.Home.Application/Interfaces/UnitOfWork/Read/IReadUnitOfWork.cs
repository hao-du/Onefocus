
using Onefocus.Home.Application.Interfaces.Repositories.Read;

namespace Onefocus.Home.Application.Interfaces.UnitOfWork.Read;

public interface IReadUnitOfWork
{
    ISettingsReadRepository Settings { get; }
}
