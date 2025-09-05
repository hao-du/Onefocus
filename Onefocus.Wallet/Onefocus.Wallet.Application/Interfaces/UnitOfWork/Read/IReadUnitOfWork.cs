using Onefocus.Wallet.Application.Interfaces.Repositories.Read;

namespace Onefocus.Wallet.Application.Interfaces.UnitOfWork.Read;

public interface IReadUnitOfWork
{
    IUserReadRepository User { get; }
    ICurrencyReadRepository Currency { get; }
    IBankReadRepository Bank { get; }
    ICounterpartyReadRepository Counterparty { get; }
    ITransactionReadRepository Transaction { get; }
}
