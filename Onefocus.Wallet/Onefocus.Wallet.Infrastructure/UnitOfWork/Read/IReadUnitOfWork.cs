using Onefocus.Wallet.Domain.Repositories.Read;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Read;

public interface IReadUnitOfWork
{
    IUserReadRepository User { get; }
    ICurrencyReadRepository Currency { get; }
    IBankReadRepository Bank { get; }
}
