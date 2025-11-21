using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Interfaces.Services;

internal interface ITransactionService
{
    Task PublishEvents(Entity.TransactionTypes.BankAccount bankAccount, CancellationToken cancellationToken);
    Task PublishEvents(Entity.TransactionTypes.CashFlow cashFlow, CancellationToken cancellationToken);
    Task PublishEvents(Entity.TransactionTypes.CurrencyExchange currencyExchange, CancellationToken cancellationToken);
    Task PublishEvents(Entity.TransactionTypes.PeerTransfer peerTransfer, CancellationToken cancellationToken);
}
